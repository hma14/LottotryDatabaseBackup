

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace SqlBackupApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .Build();

            var configuration = builder.Services.GetRequiredService<IConfiguration>();

            // Retrieve settings
            var password = configuration["Password"] ?? "";
            var server = configuration["ConnectionSettings:Server"] ?? "";
            var userId = configuration["ConnectionSettings:UserId"] ?? "";
            var trustServerCert = bool.Parse(configuration["ConnectionSettings:TrustServerCertificate"] ?? "true");
            var lottotry = configuration["DatabaseNames:lottotry"] ?? "";
            var lottotrydb = configuration["DatabaseNames:lottotrydb"] ?? "";



            string[] databaseNames = { lottotry, lottotrydb };
            string[] backupPaths =   { "F:\\Database_Backup\\Lottotry_DB_backup", "F:\\Database_Backup\\LottotryDb_DB_backup" };

            

            for (int i = 0; i < databaseNames.Length; i++) {
                string connectionString = $"Server={server};Database={databaseNames[i]};User Id={userId};Password={password};TrustServerCertificate={trustServerCert};";
                BackupDatabase(databaseNames[i], backupPaths[i], connectionString);
            }
            Console.WriteLine($"Database backup completed successfully.");
        }

        private static void BackupDatabase(string databaseName, string backupPath, string connectionString)
        {      
            using (SqlConnection connection = new(connectionString))
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