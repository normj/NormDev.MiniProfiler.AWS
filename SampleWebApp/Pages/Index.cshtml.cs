using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace SampleWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private IAmazonS3 _s3Client;
        private IAmazonDynamoDB _ddbClient;

        public IList<string> BucketNames { get; set; }
        
        public IList<string> TableNames { get; set; }

        public IndexModel(IAmazonS3 s3Client, IAmazonDynamoDB ddbClient)
        {
            _s3Client = s3Client;
            _ddbClient = ddbClient;
        }
        
        public async Task OnGet()
        {
            var listBucketsResponse = await _s3Client.ListBucketsAsync();
            
            BucketNames = new List<string>();
            listBucketsResponse.Buckets.ForEach(x => BucketNames.Add(x.BucketName));

            var tableResponse = await _ddbClient.ListTablesAsync();

            TableNames = tableResponse.TableNames;

            try
            {
                await _s3Client.GetObjectAsync("Foo", "key");
            }
            catch (Exception)
            {
            }
        }
    }
}