using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Net.Http.Json;

class Program
{
    private static readonly HttpClient client = new HttpClient();
    private static String baseUrl = "https://waba.ivosights.com/api/v1/messages/send-template-message"; //base url for sent template

    static async Task Main(string[] args)
    {
        var PAYLOAD = 
        @"{
            ""wa_id"": """",
            ""template_id"": """",
            ""components"": []
        
        }"; //template id put from waba

        Console.WriteLine($"payload: {PAYLOAD}");

        var content = new StringContent(PAYLOAD, Encoding.UTF8, "application/json");
        client.DefaultRequestHeaders.Add("X-API-KEY", ""); // Add a custom header apikey

        try
        {
            HttpResponseMessage response = await client.PostAsync(baseUrl, content); // Send the POST request
            response.EnsureSuccessStatusCode(); // Ensure the request was successful
            string responseBody = await response.Content.ReadAsStringAsync(); // Read and handle the response
            Console.WriteLine(responseBody);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
    }

}
