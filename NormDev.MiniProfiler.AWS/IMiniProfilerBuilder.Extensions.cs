
// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once InconsistentNaming
    public static class IMiniProfilerBuilderExtensions
    {
        /// <summary>
        /// Adds all AWS service calls made with the AWS SDK for .NET to the MiniProfiler trace.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IMiniProfilerBuilder AddAwsSdk(this IMiniProfilerBuilder builder)
        {
            NormDev.MiniProfiler.AWS.Register.RegisterWithSdk();
            return builder;
        }
    }
}