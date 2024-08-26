using System.Security.Cryptography;
using Microsoft.Data.Sqlite;

class ContacRepository
{
    // Define the connection string to your SQLite database
    private static string connectionString = AppConfig.GetAppSetting("Database:ConnectionString");
    private static string tableName = "Contact";

    public static void Setup() {

        // Example usage: Check if the "Contact" table exists
        bool tableExists = BaseRepository.CheckIfTableExists(connectionString, tableName);

        if (tableExists) {
            Console.WriteLine("Table 'Contact' exists.");
        } else {
            // Example usage: Creating a table with dynamic columns
            var columns = new Dictionary<string, string>
            {
                { "Id", "INTEGER PRIMARY KEY AUTOINCREMENT" },
                { "Name", "TEXT NOT NULL" },
                { "Number", "TEXT NOT NULL" },
                { "DateCreated", "DATETIME DEFAULT CURRENT_TIMESTAMP" },
                { "DateUpdated", "DATETIME NULL" },
                { "DateDeleted", "DATETIME NULL" }
            };

            BaseRepository.CreateDynamicTable(connectionString, tableName, columns);
        }

        Task.Delay(1000);
        InsertDummyContact(10);

    }

    public static void InsertDummyContact(int numberOfRows) {
        var dummyData = GenerateDummyData(numberOfRows);
        BaseRepository.InsertDynamicListData(connectionString, tableName, dummyData);
    }

    public static List<ContactEntity> getListContact() {
        var items = new List<ContactEntity>();

        try {

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // SQL SELECT query to retrieve all data from the table
                string query = "SELECT Id, Name, Number, DateCreated FROM Contact;";

                using (var command = new SqliteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ContactEntity
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Name = reader["Name"].ToString(),
                                Number = reader["Number"].ToString(),
                                DateCreated = Convert.ToDateTime(reader["DateCreated"])
                            };

                            items.Add(item);
                        }
                    }
                }
            }
        } catch (SqliteException ex) {
            Console.WriteLine($"SQLite error: {ex.Message}");
        } catch (Exception ex) {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        return items;
    }

    static List<Dictionary<string, object>> GenerateDummyData(int numberOfRows)
    {
        var dataList = new List<Dictionary<string, object>>();

        for (int i = 5; i <= numberOfRows; i++)
        {
            var data = new Dictionary<string, object>
            {
                { "Name", $"Room {i}" },
                { "Number", $"628781253810{i}" },
            };

            bool exists = BaseRepository.CheckRecordExists(connectionString, tableName, data);
            if (!exists) dataList.Add(data);
        }

        return dataList;
    }
}