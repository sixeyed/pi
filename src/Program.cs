using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pi.Web.Math;
using PowerArgs;

namespace Pi.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var arguments = Args.Parse<Arguments>(args);
                switch (arguments.Mode)
                {
                    case RunMode.Web:
                        Console.WriteLine("Running in web mode...");
                        CreateHostBuilder(args).Build().Run();
                        break;

                    case RunMode.Console:
                        Console.WriteLine(GetPi(arguments.DecimalPlaces));
                        break;

                    case RunMode.File:
                        File.WriteAllText(arguments.OutputPath, GetPi(arguments.DecimalPlaces));
                        Console.WriteLine($"Wrote pi to: {arguments.DecimalPlaces} dp; at: {arguments.OutputPath}");
                        break;
                }
            }
            catch (ArgException)
            {
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<Arguments>());
            }
        }

        private static string GetPi(int decimalPlaces)
        {
            HighPrecision.Precision = decimalPlaces;
            HighPrecision first = 4 * Atan.Calculate(5);
            HighPrecision second = Atan.Calculate(239);
            var pi = 4 * (first - second);
            return pi.ToString();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
