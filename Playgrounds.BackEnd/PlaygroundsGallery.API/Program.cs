using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Seed;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace PlaygroundsGallery.API
{
    public class Program
    {
        // private static IConfiguration _config;
        private static GalleryContext _context;
        private static bool setInitData()
        {
            var savechangesNeeded = false;
            savechangesNeeded = savechangesNeeded 
                    // || InitialSeed.SeedLocations(_context)
                    // || InitialSeed.SeedPlaygrounds(_context)
                    // || InitialSeed.SeedProfilePictures(_context)
                    
                    // Add new members with checkins to playground;
                    // var membersSeed = new MembersSeedFromJsonData(context);
                    // membersSeed.AddMembersFromJson("Data/Json/Barcelona/male-checkins2-members.data.json");
                    // membersSeed.AddMembersFromJson("Data/Json/Barcelona/female-checkins2-members.data.json");

                    // Set some checkins in today's schedule 
                    || CheckinUpdate.UpdateTodayCheckins(_context);
            return savechangesNeeded;
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                // .ConfigureAppConfiguration((hostingContext, config) =>
                //     {
                //         if (config != null) 
                //         {
                //             _config = config.Build();
                //             config.AddAzureAppConfiguration(options => {
                //                 options.ConfigureClientOptions(configOption => {
                //                     configOption.
                //                 })
                //             });
                //             // config.AddAzureAppConfiguration(_config["AzureLogAnalytics:workspaceId"]);
                //             // config.AddAzureAppConfiguration(_config["AzureLogAnalytics:authenticationId"]);
                //         }
                //     })
                .UseStartup<Startup>()
                .UseSerilog();
        
        public static void Main(string[] args)
        {
            IWebHost host = null;
            try
            {
                // var config = GetConfiguration();
                host = CreateWebHostBuilder(args).Build();
            }    
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }    

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    _context = services.GetRequiredService<GalleryContext>();
                    _context.CurrentDatabaseMigrate();
                    if (setInitData()) {
                        Log.Information("data base updated at start");
                        InitialSeed.SaveSeeds(_context);
                    } 
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "An error occuried during Migration");
                }
            }

            host.Run();
        }
        
        // private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        // {
        //     var devLog = configuration["Serilog:LogstashgUrl"];
        //     var workspaceIdValue = _config.GetSection("AzureLogAnalytics:workspaceId")?.Value;
        //     var authenticationIdValue = _config.GetSection("AzureLogAnalytics:authenticationId")?.Value;
        //     if (!string.IsNullOrEmpty(workspaceIdValue) && !string.IsNullOrEmpty(authenticationIdValue))
        //     {
        //         return new LoggerConfiguration()
        //             .MinimumLevel.Debug()
        //             .WriteTo.AzureAnalytics(
        //                 workspaceId: workspaceIdValue,
        //                 authenticationId: authenticationIdValue)
        //             .ReadFrom.Configuration(_config)
        //             .CreateLogger();
        //     }

        //     return new LoggerConfiguration()
        //         .MinimumLevel.Debug()
        //         .ReadFrom.Configuration(_config)
        //         .CreateLogger();
        // }

        // private static IConfiguration GetConfiguration()
        // {
        //     var builder = new ConfigurationBuilder()
        //         .SetBasePath(Directory.GetCurrentDirectory())
        //         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //         .AddEnvironmentVariables();

        //     return builder.Build();
        // }
        
        // public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        // {
        //     return WebHost.CreateDefaultBuilder(args)
        //         .UseStartup<Startup>()
        //         .UseSerilog();
        // }
    }
}
