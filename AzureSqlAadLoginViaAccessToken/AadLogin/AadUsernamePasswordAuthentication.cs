using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AadLogin
{
    class AadUsernamePasswordAuthentication
    {
        static void Main(string[] args)
        {
            AadUsernamePasswordAuthentication program = new AadUsernamePasswordAuthentication();

            Task<string> task = program.SqlServerVersion();
            task.Wait();

            var result = task.Result;

            Console.WriteLine(result);
            Console.ReadKey();

        }
        private async Task<string> SqlServerVersion()
        {
            var provider = new AzureServiceTokenProvider();

            SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder
            {
                DataSource = "anshriv-azure-db.database.windows.net",
                InitialCatalog = "anubhav",
                UserID = "****************",
                Password = "*************"

            };

            csb.Authentication = SqlAuthenticationMethod.ActiveDirectoryPassword;

            using (var conn = new SqlConnection(csb.ConnectionString))
            {

                await conn.OpenAsync().ConfigureAwait(false);

                using (var sqlCommand = new SqlCommand("select * from student", conn))
                {
                    SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                    while (await sqlDataReader.ReadAsync())
                    {
                        int fieldCount = sqlDataReader.FieldCount;
                        for (int i = 0; i < fieldCount; i++)
                        {
                            Console.Write(sqlDataReader[i]);
                            Console.Write(" ");
                        }
                        Console.WriteLine();
                    }
                }
            }
            return "";
        }
    }
}
