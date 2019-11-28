using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using Threax.AspNetCore.BuiltInTools;

namespace Threax.Keepass
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var tools = new ToolManager(args);
            var host = BuildWebHostWithConfig(tools.GetCleanArgs(), tools.GetEnvironment());

            if (tools.ProcessTools(host))
            {
                host.Run();
            }
        }

        /// <summary>
        /// This version of BuildWebHost allows entity framework commands to work correctly.
        /// </summary>
        /// <param name="args">The args to use for the builder.</param>
        /// <returns>The constructed IWebHost.</returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            return BuildWebHostWithConfig(args);
        }

        /// <summary>
        /// The is the real build web host function, you can specify a config name to force load or leave it null to use the current environment.
        /// </summary>
        /// <param name="args">The args to use for the builder.</param>
        /// <param name="toolsConfigName">The name of the tools config to load, or null to not load these configs.</param>
        /// <returns>The constructed IWebHost.</returns>
        public static IWebHost BuildWebHostWithConfig(string[] args, String toolsConfigName = null)
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var env = hostContext.HostingEnvironment;
                    config.Sources.Clear();

                    //./appsettings.json - Main settings file, shared between all instances
                    config.AddJsonFileWithInclude("appsettings.json", optional: true, reloadOnChange: true);

                    //./appsettings.{environment}.json - Local development settings files, loaded per environment, no need to deploy to server
                    config.AddJsonFileWithInclude($"appsettings.{env.EnvironmentName}.json", optional: true);

                    //./appsettings.tools.json - Local development tools settings files, loaded in tools mode, no need to deploy to server
                    if (toolsConfigName != null)
                    {
                        config.AddJsonFileWithInclude($"appsettings.{toolsConfigName}.json", optional: true);
                    }

                    //./../appsettings.{environment}.json - Deployed settings file, loaded per environment, allows you to put the production configs 1 level above the site in produciton, which keeps that config separate from the code
                    config.AddJsonFileWithInclude(Path.GetFullPath($"../appsettings.{env.EnvironmentName}.json"), optional: true, reloadOnChange: true);

                    //./../appsettings.tools.json - Deployed tools settings file, loaded in tools mode, allows you to put the production tools configs 1 level above the site in produciton, which keeps that config separate from the code
                    if (toolsConfigName != null)
                    {
                        config.AddJsonFileWithInclude(Path.GetFullPath($"../appsettings.{toolsConfigName}.json"), optional: true);
                    }

                    //Secrets
                    if (File.Exists("appsettings.secrets.json"))
                    {
                        config.AddJsonFileWithInclude(Path.GetFullPath("appsettings.secrets.json"), optional: false);
                    }
                    else
                    {
                        config.AddUserSecrets<Program>();
                    }

                    //Environment variables
                    config.AddEnvironmentVariables();
                    config.AddCommandLine(args);
                });

            return webHostBuilder.Build();
        }
    }
}
