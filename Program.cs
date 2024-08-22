using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Net.Http.Json;
using System.IO.Enumeration;
using System.Threading.Tasks;

class Program
{
    public static readonly HttpClient client = new HttpClient();
    public static String baseUrl = "https://waba.ivosights.com/api/v1/messages/send-template-message"; //base url for sent template
    public record Contact(int Id, string Name, string Number);
    public record Template(string wa_id, string template_id, string[] components);
    public static String template_id = "66949932db020a0e202048ae";

    static async Task Main(string[] args)
    {

        //list contact
        var items = new List<Contact>
        {
            new Contact(1, "Didik", "6287812538105"),
            new Contact(2, "Afghan", "6289614786396")
        };

        // list payload message
        var payload = new List<Template>{};
        foreach (var item in items)
        {
            Template temp = new Template(item.Number, Program.template_id, []);
            payload.Add(temp);
        }

        // send message with interval
        await ProcessSendMessagesWithIntervalAsync(payload, chunkSize: 1, intervalMilliseconds: 2000);


    }

    static async Task ProcessSendMessagesWithIntervalAsync(List<Template> items, int chunkSize, int intervalMilliseconds)
    {
        var chunks = items.Chunk(chunkSize);
        Console.WriteLine($"Processing items: {items}");

        foreach (var item in items)
        {
            // Process each chunk
            Console.WriteLine($"Processing chunk: {item}");
            await SendMessage(item);

            // Simulate processing (replace this with your actual logic)
            await Task.Delay(500); // Simulate some work per chunk

            // Wait for the specified interval before processing the next chunk
            await Task.Delay(intervalMilliseconds);
        }
    }

    static async Task SendMessage(Template payload) {

        var jsonData = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        try
        {
            client.DefaultRequestHeaders.Add("X-API-KEY", "1t65d576f2af2be5.64427625ToYdd03"); // Add a custom header apikey
            var response = await client.PostAsync(Program.baseUrl, content);
            Console.WriteLine(response);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
    }

}
