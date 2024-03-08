/*using System.Net;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }

        [Function("WeatherForecast")]
        public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var randomNumber = new Random();
            var temp = 0;

            var result = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = temp = randomNumber.Next(-20, 55),
                Summary = GetSummary(temp)
            }).ToArray();

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(result);

            return response;
        }

        private string GetSummary(int temp)
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
}*/
using System.IO;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared;

namespace Api
{
	public class HttpTrigger
	{
		private readonly ILogger _logger;

		public HttpTrigger(ILoggerFactory loggerFactory)
		{
			_logger = loggerFactory.CreateLogger<HttpTrigger>();
		}

		[Function("WeatherForecast")]
		public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
		{
			// Path to the JSON file
			var jsonFilePath = Path.Combine(Environment.CurrentDirectory, "data.json");

			// Read the JSON file content
			var jsonData = File.ReadAllText(jsonFilePath);

			// Deserialize the JSON content to an array of WeatherForecast objects
			var forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(jsonData);

			// Create the HTTP response and write the serialized JSON content
			var response = req.CreateResponse(HttpStatusCode.OK);
			response.WriteAsJsonAsync(forecasts);

			return response;
		}

        [Function("Flashcard")]
        public HttpResponseData Run2([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Path to the JSON file
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory, "flashcardData.json");

            // Read the JSON file content
            var jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON content to an array of WeatherForecast objects
            var flashcards = JsonSerializer.Deserialize<Flashcard[]>(jsonData);

            // Create the HTTP response and write the serialized JSON content
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(flashcards);

            return response;
        }
    }
}