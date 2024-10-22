

using Microsoft.Data.SqlClient;

namespace SqlBackupApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] databaseNames = { "lottotry", "lottotrydb" };
            string[] backupPaths =   { "G:\\Database_Backup\\Lottotry_DB_backup", "G:\\Database_Backup\\LottotryDb_DB_backup" };

            for (int i = 0; i < databaseNames.Length; i++) {

                BackupDatabase(databaseNames[i], backupPaths[i]);
            }
            Console.WriteLine($"Database backup completed successfully.");
        }

        private static void BackupDatabase(string databaseName, string backupPath)
        {
            string connectionString = $"Server=webserver;Database={databaseName};User Id=sa;Password=Bilibalabon12345;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string backupFileName = $"{backupPath}\\{databaseName}_backup.bak";
                    string backupQuery = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFileName}' WITH FORMAT;";

                    using (SqlCommand command = new SqlCommand(backupQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine($"Database backup completed successfully for {databaseName}.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during backup: {ex.Message}");
                }
            }
        }
    }
}