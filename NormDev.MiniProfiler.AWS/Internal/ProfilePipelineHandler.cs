using System;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using StackExchange.Profiling;
using StackExchange.Profiling.Helpers;


namespace NormDev.MiniProfiler.AWS.Internal
{
    /// <summary>
    /// The design AWS SDK for .NET uses a pipeline made of a series of pipeline handlers to handle each stage of
    /// a request. This handler is injected into the pipeline to capture the service and operation call and initiate a
    /// step in the current MiniProfiler trace. 
    /// </summary>
    public class ProfilePipelineHandler : PipelineHandler
    {
        public override async Task<T> InvokeAsync<T>(IExecutionContext executionContext)
        {
            var requestContext = executionContext.RequestContext;
            var serviceName = requestContext.Request?.ServiceName;
            var operation = requestContext.OriginalRequest?.GetType().Name;

            string stepName;
            if (!string.IsNullOrEmpty(serviceName) && !string.IsNullOrEmpty(operation))
            {
                if (serviceName.StartsWith("Amazon."))
                    serviceName = serviceName.Substring("Amazon.".Length);
                else if (serviceName.StartsWith("Amazon"))
                    serviceName = serviceName.Substring("Amazon".Length);
                if (operation.EndsWith("Request"))
                    operation = operation.Substring(0, operation.Length - "Request".Length);

                stepName = $"AWS SDK - {serviceName}.{operation}";
            }
            else
            {
                stepName = "Unknown AWS Call";
            }

            var profiler = StackExchange.Profiling.MiniProfiler.Current;
            if (profiler != null)
            {
                using (var timing = profiler.Step(stepName))
                {
                    return await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
                }
            }
            else
            {
                return await base.InvokeAsync<T>(executionContext).ConfigureAwait(false);
            }
        }
    }
}