using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HighCPUFunc561
{
    public class http1
    {
        private readonly ILogger _logger;
        private readonly IHighCPUService _highCPUService;
        private bool _isRunning = false;
        public static CancellationTokenSource cts;

        public http1(ILoggerFactory loggerFactory, IHighCPUService highCPUService)
        {
            _logger = loggerFactory.CreateLogger<http1>();
            _highCPUService = highCPUService;
        }

        [Function("http1")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var tempCts = new CancellationTokenSource();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
