using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Sample.Dto;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace Sample.Api
{
    public class WeatherForecastFunctions
    {
        /**********/
        /* Fields */
        /**********/

        private readonly ILogger _logger;

        /****************/
        /* Constructors */
        /****************/

        public WeatherForecastFunctions(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<WeatherForecastFunctions>();
        }

        /***********/
        /* Methods */
        /***********/

        [Function("WeatherForecast")]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var result = GetData();
            var response = req.CreateResponse();
            await response.WriteAsJsonAsync(result);
            return response;
        }


        private static WeatherForecast[] GetData()
        {
            var randomNumber = new Random();
            var temp = 0;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();
        }


        private static string GetSummary(int temp)
        {
            var summary = "Mild";

            if (temp >= 32)
            {
                summary = "Hot";
            }
            else if (temp <= 16 && temp > 0)
            {
                summary = "Cold";
            }
            else if (temp <= 0)
            {
                summary = "Freezing";
            }

            return summary;
        }
    }
}