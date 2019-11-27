using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AadLogin
{
    class Program
    {
        static void Main(string[] args)
        {
            Program program = new Program();

            Task<string> task = program.SqlServerVersion();
            task.Wait();

            var result =task.Result;

            Console.WriteLine(result);
            Console.ReadKey();

        }
        private async Task<string> SqlServerVersion()
        {
            var provider = new AzureServiceTokenProvider();
            var token = await provider.GetAccessTokenAsync("https://database.windows.net/", "anubhavworkemailgmail.onmicrosoft.com").ConfigureAwait(false);

            Console.WriteLine(token);

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder
            {
                DataSource = "anshriv-azure-db.database.windows.net",
                InitialCatalog = "anubhav"
            };

        using (var conn = new SqlConnection(csb.ConnectionString))
        {
            conn.AccessToken = token;
            await conn.OpenAsync().ConfigureAwait(false);

            using (var sqlCommand = new SqlCommand("SELECT @@VERSION", conn))
            {
                var result = await sqlCommand.ExecuteScalarAsync().ConfigureAwait(false);
                return result.ToString();
            }
        }
    }


    }
}
