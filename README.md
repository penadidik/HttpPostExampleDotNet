## Quick Start HTTP Client POST Example for WhatsApp Business API

1. **Install dotnet 8 to test this sample:**

    Download and Install [dotNet](https://dotnet.microsoft.com/en-us/download).

2. **Supporting Tools:**

    Please install Visual Studio Code to run this code or you can reuse this code in your Vb.Net.

3. **Run HTTP Post:**

    Run the following command to install required dependencies using dotnet:

    ```bash
    dotnet run
    ```

4. **Log success:**

    Below is log already print if success:

    ```bash
    payload: { "wa_id": "***********", "template_id": "**************", "components": [] }
    {"messaging_product":"whatsapp","contacts":[{"input":"**********","wa_id":"********"}],"messages":[{"id":"***************","message_status":"accepted"}]}
    ```

    Now, your project is ready for use. You can access the admin panel via the provided route. If you've run the seed command, log in with the provided credentials. Customize and expand your application as needed. Check code in Program.cs (it's just sample code).
