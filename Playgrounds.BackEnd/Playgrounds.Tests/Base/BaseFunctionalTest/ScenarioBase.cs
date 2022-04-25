using PlaygroundsGallery.DataEF;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using System;
using PlaygroundsGallery.API;

namespace Playgrounds.FunctionalTests
{
    public class ScenarioBase
    {
        // private GalleryContext _context;

        public TestServer CreateServer()
        {
            var path = Assembly.GetAssembly(typeof(ScenarioBase)).Location;
            
            string projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var hostBuilder = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    // var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false)
                    .AddEnvironmentVariables();
                })
                .UseKestrel()
                .UseEnvironment("Test")
                
                // .UseStartup<PlaygroundsTestStartup>();
                .UseStartup<Startup>();


            var testServer = new TestServer(hostBuilder);
            
            // testServer.Host
            //     .MigrateDbContext<GalleryContext>((context, services) =>
            //     {
            //         // var env = services.GetService<IWebHostEnvironment>();
            //         // var settings = services.GetService<IOptions<CatalogSettings>>();
            //         // var logger = services.GetService<ILogger<CatalogContextSeed>>();

            //         // new CatalogContextSeed()
            //         // .SeedAsync(context, env, settings, logger)
            //         // .Wait();
            //     });

            return testServer;
        }

        public static class Get
        {
            public static string ItemById(int id)
            {
                return $"api/playgrounds/{id}";
            }
        }

        public static class Post
        {
            public static string Auth = $"api/auth/login";
            
        }
    }
}