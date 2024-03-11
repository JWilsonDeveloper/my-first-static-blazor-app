using System.IO;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using BlazorApp.Shared;
using System.Runtime.InteropServices;
using System.Web;

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

        [Function("GetAllFlashcards")]
        public HttpResponseData GetAllFlashcards([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Path to the JSON file
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory, "AllFlashcardData.json");

            // Read the JSON file content
            var jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON content to an array of Flashcard objects
            var flashcards = JsonSerializer.Deserialize<Flashcard[]>(jsonData);

            // Create the HTTP response and write the serialized JSON content
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(flashcards);

            return response;
        }

        [Function("FlashcardCategories")]
        public HttpResponseData FlashcardCategories([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Path to the JSON file
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory, "AllFlashcardData.json");

            // Read the JSON file content
            var jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON data into a list of objects
            var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(jsonData);

            // Extract the categories from each object
            var categoryArrays = flashcards.Select(flashcard => flashcard.Category).Distinct().ToList();

            // Create the HTTP response and write the serialized JSON content
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(categoryArrays);

            return response;
        }

        [Function("FlashcardsByCategories")]
        public HttpResponseData FlashcardsByCategories([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            // Path to the JSON file
            var jsonFilePath = Path.Combine(Environment.CurrentDirectory, "AllFlashcardData.json");

            // Read the JSON file content
            var jsonData = File.ReadAllText(jsonFilePath);

            // Deserialize the JSON data into a list of objects
            var flashcards = JsonSerializer.Deserialize<List<Flashcard>>(jsonData);

            // Get the query parameter from the request (assuming it's named "categories")
            var query = req.Url.Query; // Example: "?categories=Category1%Category2%Category3"
            Console.WriteLine("query: " + query);
            var queryParameters = HttpUtility.ParseQueryString(query);
            var categories = queryParameters["categories"]?.Split('%'); // Split by percent sign

            if (categories == null || categories.Length == 0)
            {
                // No categories provided, return an empty list
                var emptyResponse = req.CreateResponse(HttpStatusCode.OK);
                emptyResponse.WriteAsJsonAsync(new List<string>());
                return emptyResponse;
            }

            // Extract flashcards with Category property that matches one of the delineated strings
            var matchingFlashcards = flashcards.Where(flashcard => categories.Contains(flashcard.Category)).ToList();

            // Create the HTTP response and write the serialized JSON content
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(matchingFlashcards);

            return response;
        }
    }
}