using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace HighCPUFunc561
{
    public class http2
    {
        private readonly ILogger _logger;
        private readonly IHighCPUService _highCPUService;

        public http2(ILoggerFactory loggerFactory, IHighCPUService highCPUService)
        {
            _logger = loggerFactory.CreateLogger<http2>();
            _highCPUService = highCPUService;
        }

        [Function("http2")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _highCPUService.CancelHighCPU();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            response.WriteString("Welcome to Azure Functions!");

            return response;
        }
    }
}
