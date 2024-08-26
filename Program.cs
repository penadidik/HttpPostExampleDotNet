using System;

class Program
{

    static async Task Main(string[] args)
    {

        // Initialize configuration
        AppConfig.Initialize("appsettings.json");

        var collectContact = ContactController.GetListFromDB();
        var templates = TemplateController.SetTemplateTesting(collectContact);

        // send message with interval
        await ProcessSendMessagesWithIntervalAsync(templates, chunkSize: 2, intervalMilliseconds: 2000);

    }

    static async Task ProcessSendMessagesWithIntervalAsync(List<TemplateEntity> items, int chunkSize, int intervalMilliseconds)
    {

        List<List<TemplateEntity>> splitLists = Split.ListToChunk(items, chunkSize); // make queue

        // Output to verify the split and send template
        int i = 1;
        foreach (var list in splitLists)
        {
            Console.WriteLine($"List {i}:");
            foreach (var template in list)
            {
                Console.WriteLine($"  Id: {template.Id}, Name: {template.wa_id}, Components: {template.components}");
                await Ivowaba.SendMessage(template); // Process each chunk
                await Task.Delay(500); // Simulate some work per chunk
                await Task.Delay(intervalMilliseconds); // Wait for the specified interval before processing the next chunk
            }
            i++;
        }
    }

}
