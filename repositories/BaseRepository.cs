using Microsoft.Data.Sqlite;

class BaseRepository
{
    public static void CreateDynamicTable(string connectionString, string tableName, Dictionary<string, string> columns) {
        try {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Start building the SQL query
                string createTableQuery = $"CREATE TABLE IF NOT EXISTS {tableName} (";

                // Add columns to the query
                foreach (var column in columns)
                {
                    createTableQuery += $"{column.Key} {column.Value}, ";
                }

                // Remove the trailing comma and space, then add the closing parenthesis
                createTableQuery = createTableQuery.TrimEnd(',', ' ') + ");";

                // Execute the SQL command
                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Table '{tableName}' created successfully.");
                }
            }
        } catch (SqliteException ex) {
            Console.WriteLine($"SQLite error: {ex.Message}");
        } catch (Exception ex) {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        
    }

    public static bool CheckIfTableExists(string connectionString, string tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Query to check if the table exists in the sqlite_master table
            string query = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name=@tableName;";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@tableName", tableName);

                // Execute the query and get the result
                long tableCount = (long)command.ExecuteScalar();

                // If count > 0, the table exists
                return tableCount > 0;
            }
        }
    }

    public static void InsertDynamicData(string connectionString, string tableName, Dictionary<string, object> data) {

        try {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Start building the SQL INSERT query
                string columns = string.Join(", ", data.Keys);
                string values = string.Join(", ", data.Keys.Select(key => $"@{key}"));

                string insertQuery = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    // Add parameters to the command
                    foreach (var kvp in data)
                    {
                        command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                    }

                    // Execute the command
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} row(s) inserted into {tableName}.");
                }
            }
        } catch (SqliteException ex) {
            Console.WriteLine($"SQLite error: {ex.Message}");
        } catch (Exception ex) {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static void InsertDynamicListData(string connectionString, string tableName, List<Dictionary<string, object>> rows)
    {
        try {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                if (rows.Count == 0)
                {
                    Console.WriteLine("No data to insert.");
                    return;
                }

                // Extract column names from the first dictionary
                var columns = rows.First().Keys.ToList();
                string columnNames = string.Join(", ", columns);
                string parameterNames = string.Join(", ", columns.Select(key => $"@{key}"));

                // SQL INSERT query
                string insertQuery = $"INSERT INTO {tableName} ({columnNames}) VALUES ({parameterNames});";

                using (var transaction = connection.BeginTransaction())
                {
                    using (var command = new SqliteCommand(insertQuery, connection))
                    {
                        // Associate command with the transaction
                        command.Transaction = transaction;

                        foreach (var row in rows)
                        {
                            command.Parameters.Clear(); // Clear previous parameters

                            // Add parameters for the current row
                            foreach (var column in columns)
                            {
                                command.Parameters.AddWithValue($"@{column}", row[column]);
                            }

                            // Execute the command for the current row
                            command.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                }

                Console.WriteLine($"{rows.Count} row(s) inserted into {tableName}.");
            }
        } catch (SqliteException ex) {
            Console.WriteLine($"SQLite error: {ex.Message}");
        } catch (Exception ex) {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public static List<Dictionary<string, object>> GetListData(string connectionString, string tableName)
    {
        var result = new List<Dictionary<string, object>>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // SQL SELECT query to retrieve all data from the table
            string selectQuery = $"SELECT * FROM {tableName};";

            using (var command = new SqliteCommand(selectQuery, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    // Retrieve column names
                    var columnNames = Enumerable.Range(0, reader.FieldCount)
                                                .Select(i => reader.GetName(i))
                                                .ToList();

                    // Read rows
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();

                        foreach (var columnName in columnNames)
                        {
                            row[columnName] = reader[columnName];
                        }

                        result.Add(row);
                    }
                }
            }
        }

        return result;
    }

    public static bool CheckRecordExists(string connectionString, string tableName, Dictionary<string, object> columnsAndValues)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            if (columnsAndValues.Count == 0)
            {
                throw new ArgumentException("Columns and values cannot be empty.");
            }

            // Construct the SQL query dynamically based on the provided columns and values
            var conditions = columnsAndValues.Keys.Select(key => $"{key} = @{key}").ToList();
            string query = $"SELECT 1 FROM {tableName} WHERE {string.Join(" AND ", conditions)} LIMIT 1;";

            using (var command = new SqliteCommand(query, connection))
            {
                // Add parameters for each column and value
                foreach (var kvp in columnsAndValues)
                {
                    command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                }

                // Execute the query
                object result = command.ExecuteScalar();

                // If result is not null, the record exists
                return result != null;
            }
        }
    }

    public static List<Dictionary<string, object>> CheckListRecordExist(string connectionString, string tableName, List<Dictionary<string, object>> criteriaList)
    {
        var existingRecords = new List<Dictionary<string, object>>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            foreach (var criteria in criteriaList)
            {
                if (criteria.Count == 0)
                {
                    throw new ArgumentException("Criteria cannot be empty.");
                }

                // Construct the SQL query dynamically based on the provided columns and values
                var conditions = criteria.Keys.Select(key => $"{key} = @{key}").ToList();
                string query = $"SELECT * FROM {tableName} WHERE {string.Join(" AND ", conditions)} LIMIT 1;";

                using (var command = new SqliteCommand(query, connection))
                {
                    // Add parameters for each column and value
                    foreach (var kvp in criteria)
                    {
                        command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            var record = new Dictionary<string, object>();

                            foreach (var key in criteria.Keys)
                            {
                                record[key] = reader[key];
                            }

                            existingRecords.Add(record);
                        }
                    }
                }
            }
        }

        return existingRecords;
    }
}