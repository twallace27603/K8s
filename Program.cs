using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;


namespace K8s
{
    public class Program
    {
        public static double LoadSeconds = 0.0;
        public static bool LoadProcessor = false;

        private static Mutex _mutex = new Mutex();
        public static void Main(string[] args)
        {
            Task.Run(RunProcessor);
            CreateHostBuilder(args).Build().Run();
            Console.WriteLine("Through Main");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        private static void RunProcessor()
        {
            while (true)
            {
                Thread.Sleep(1000);
                bool run = false;
                double seconds = 0.0;
                _mutex.WaitOne();
                run = LoadProcessor;
                seconds = LoadSeconds;
                _mutex.ReleaseMutex();
                if (run)
                {
                    SetLoadData(false, 0);

                    var runUntil = DateTime.Now.AddSeconds(seconds);
                    Console.WriteLine($"Processor loaded at {DateTime.Now} until {runUntil}.");
                    List<Task> tasks = new List<Task>();
                    for (int lcv = 0; lcv < 100; lcv++)
                    {
                        tasks.Add(Task.Run(() =>
                        {
                            var i = 0;
                            double fact = 1;
                            while (DateTime.Now < runUntil)
                            {
                                i++;
                                fact *= (double)i;
                                if (i > int.MaxValue / 2) { i = 0; fact = 1; }
                            }
                        }));
                    }
                    Task.WaitAll(tasks.ToArray());
                    Console.WriteLine($"Processor load ended at {DateTime.Now}");
                }

            }



        }
        public static void SetLoadData(bool loadProcessor, double loadSeconds)
        {
            _mutex.WaitOne();
            LoadProcessor = loadProcessor;
            LoadSeconds = loadSeconds;
            _mutex.ReleaseMutex();
        }
    }

}
