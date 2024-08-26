using System.Text;
using System.Text.Json;

class Ivowaba
{

    private static readonly HttpClient client = new HttpClient();
    private static String baseUrl = "https://waba.ivosights.com/api/v1/messages/send-template-message"; //base url for sent template
    private static String Apikey = "1t65d576f2af2be5.64427625ToYdd03";

    public static async Task SendMessage(TemplateEntity payload) {

        var jsonData = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        try
        {
            client.DefaultRequestHeaders.Add("X-API-KEY", Apikey); // Add a custom header apikey
            var response = await client.PostAsync(baseUrl, content);
            Console.WriteLine(response);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
    }
}