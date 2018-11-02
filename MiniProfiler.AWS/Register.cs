using System;
using Amazon.Runtime;
using Amazon.Runtime.Internal;

using MiniProfiler.AWS.Internal;

namespace MiniProfiler.AWS
{
    /// <summary>
    /// This class handles registering MiniProfiler into the AWS SDK for .NET pipeline. 
    /// </summary>
    public class Register : IRuntimePipelineCustomizer
    {
        private const string CustomizerName = "MiniProfiler.AWS Registration";
        private Register()
        {
            
        }
        
        /// <summary>
        /// Registers MiniProfile with the AWS SDK for .NET so all service clients created after this registration
        /// will add AWS service calls to the MiniProfile traces.
        /// </summary>
        public static void RegisterWithSdk()
        {
            var register = new Register();
            RuntimePipelineCustomizerRegistry.Instance.Register(register);
        }

        /// <summary>
        /// Removes the MiniProfiler registration with the AWS SDK for .NET pipeline.
        /// </summary>
        // ReSharper disable once IdentifierTypo
        public static void DeregisterWithSdk()
        {
            RuntimePipelineCustomizerRegistry.Instance.Deregister(CustomizerName);
        }
        
        /// <summary>
        /// The unique name for the customizer to ensure it is only registered once.
        /// </summary>
        public string UniqueName => CustomizerName;

        /// <summary>
        /// This method is called whenever an AWS service client is instantiated.
        /// </summary>
        /// <param name="serviceClientType"></param>
        /// <param name="pipeline"></param>
        public void Customize(Type serviceClientType, RuntimePipeline pipeline)
        {
            if (serviceClientType.BaseType != typeof(AmazonServiceClient))
                return;

            pipeline.AddHandlerAfter<EndpointResolver>(new ProfilePipelineHandler());
            
        }        
    }
}