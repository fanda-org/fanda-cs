using System;
using System.Diagnostics;
using System.Net.Http;

namespace Fanda.Tests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Console.Write("Initializing..");

                var sw = Stopwatch.StartNew();
                HttpClient httpClient = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:57071/")
                };

                Console.WriteLine();
                Console.Write("Logging In..\t");
                //AccountsClient accountsClient = new AccountsClient(httpClient);
                //var signInResult = accountsClient.LoginAsync(new ViewModel.Access.LoginViewModel
                //{
                //    NameOrEmail = "fandaadmin",
                //    Password = "Welcome@123",
                //    RememberMe = true
                //}).GetAwaiter().GetResult();
                sw.Stop();
                Console.Write($" Duration: { sw.ElapsedMilliseconds} ms");

                sw.Restart();
                Console.WriteLine();
                Console.Write("Fetching orgs..\t");
                //ApiClient.ApiClient apiClient = new ApiClient.ApiClient(httpClient);
                //var orgs = apiClient.OrganizationsGetAsync().GetAwaiter().GetResult();
                Console.Write($" Duration: {sw.ElapsedMilliseconds} ms");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}