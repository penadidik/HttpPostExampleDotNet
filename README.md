## Quick Start HTTP Client POST Example

1. **Install dotnet 8 to test this sample:**

    Download and Install [dotNet](https://dotnet.microsoft.com/en-us/download).

2. **Supporting Tools:**

    Please install Visual Studio Code to run this code or you can reuse this code in your Vb.Net.

3. **Run HTTP Post:**

    Run the following command to install required dependencies using Composer:

    ```bash
    dotnet run
    ```

4. **Log success:**
   Initialize and seed the database with default data using:

    ```bash
    payload: { "wa_id": "6287812538105", "template_id": "66949932db020a0e202048ae", "components": [] } {"messaging_product":"whatsapp","contacts":[{"input":"6287812538105","wa_id":"6287812538105"}],"messages":[{"id":"wamid.HBgNNjI4NzgxMjUzODEwNRUCABEYEjBFQzkwNTA0MDY4MzI4MTYxQgA=","message_status":"accepted"}]}
    ```

    Now, your project is ready for use. You can access the admin panel via the provided route. If you've run the seed command, log in with the provided credentials. Customize and expand your application as needed.