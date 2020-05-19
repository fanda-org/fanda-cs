using Fanda.Repository;
using Fanda.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fanda.Tests
{
    internal class Program
    {
        private const int UserCount = 100;
        private const int ThreadCount = 100;

        private static void Main(string[] args)
        {
            AppSettings settings = new AppSettings
            {
                DatabaseType = "MSSQL",
                ConnectionStrings = new ConnectionStrings
                {
                    MsSqlConnection =
                    "Server=(local)\\sqlexpress;Database=Fanda;UID=sa;PWD=tbm@1234;Application Name=Fanda;Connection Lifetime=0;Min Pool Size=10;Max Pool Size=100;Pooling=true;"
                }
            };
            SerialNumberRepository repo = new SerialNumberRepository(settings);
            Guid yearId = new Guid("639CCA31-D61A-49B5-BFC4-424200C3F5F5");

            Console.WriteLine("Starting..");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DoRequests(repo, yearId);
            Console.WriteLine("Elapsed: {0}",sw.Elapsed);
            Console.WriteLine("Finished.");
        }

        private static void DoRequests(SerialNumberRepository repo, Guid yearId)
        {
            SemaphoreSlim throtller = new SemaphoreSlim(ThreadCount);
            List<Task> tasks = new List<Task>();
            Parallel.For(0, UserCount, n =>
            {
                tasks.Add(Task.Run(() =>
                {
                    try
                    {
                        throtller.Wait();
                        NextNum(repo, yearId);
                    }
                    finally
                    {
                        throtller.Release();
                    }
                }));
            });
            Task.WaitAll(tasks.ToArray());

            //List<requestInfo> requestInfoList = new List<requestInfo>();
            //for (int i = 0; dataRequestInfo.RowCount - 1 > i; i++)
            //{
            //    requestInfoList.Add((requestInfo)dataRequestInfo.Rows[i].Tag);
            //}
            //var requestInfoList = new List<Task>();
            //for (int i = 0; i < UserCount; i++)
            //{
            //    requestInfoList.Add(NextNum(repo, yearId));
            //}
            //await Task.WhenAll(requestInfoList.Select(async task =>
            //{
            //    await maxThread.WaitAsync();
            //    try
            //    {
            //        await task;
            //    }
            //    finally
            //    {
            //        maxThread.Release();
            //    }
            //}));
        }

        private static void NextNum(SerialNumberRepository repo, Guid yearId)
        {
            var batchNumber = repo.NextNumber(yearId, SerialNumberModule.BatchNumber);
            //Thread.Sleep(100);
            Console.WriteLine("{0} - {1}", batchNumber, DateTime.Now.ToString("ss:fff"));
        }
    }
}
