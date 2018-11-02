using System;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Profiling;
using Xunit;

using Amazon;
using Amazon.S3;

namespace MiniProfiler.AWS.Tests
{
    using MiniProfiler = StackExchange.Profiling.MiniProfiler;
    
    public class SdkRegistrationTests
    {
        [Fact]
        public async Task RegisterSdk()
        {
            Register.RegisterWithSdk();
            try
            {
                var profiler = MiniProfiler.StartNew("Test Trace");
                using (profiler.Step("Start"))
                {
                    await ListBuckets();
                }

                profiler.Stop();
                var awsTiming = profiler.GetTimingHierarchy().FirstOrDefault(x => string.Equals(x.Name, "AWS SDK - S3.ListBuckets"));
                Assert.NotNull(awsTiming);
            }
            finally
            {
                Register.DeregisterWithSdk();
            }

            {
                var profiler = MiniProfiler.StartNew("Test Trace");
                using (profiler.Step("Start"))
                {
                    await ListBuckets();
                }

                profiler.Stop();
                var awsTiming = profiler.GetTimingHierarchy().FirstOrDefault(x => string.Equals(x.Name, "AWS SDK - S3.ListBuckets"));
                Assert.Null(awsTiming);                
            }
        }


        private static async Task ListBuckets()
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                await client.ListBucketsAsync();
            }            
        }
    }
}