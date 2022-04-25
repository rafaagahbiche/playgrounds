using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Seed;
using Microsoft.Extensions.Hosting;
using PlaygroundsGallery.DataEF.SeedDataSource;

namespace PlaygroundsGallery.API
{
    public class Program
    {
        // private static IConfiguration _config;
        private static GalleryContext _context;

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
        
        public static void Main(string[] args)
        {
            IWebHost host = null;
            try
            {
                host = BuildWebHost(args);
            }    
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }    

            if (host != null)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        _context = services.GetRequiredService<GalleryContext>();
                        _context.CurrentDatabaseMigrate();
                        // var locationSeed = new LocationSeed();
                        // locationSeed.Seed(_context);
                        // var playgroundsSeed = new PlaygroundSeed();
                        // playgroundsSeed.Seed(_context);
                        var membersSeed = new MembersSeedFromJsonData(_context);
                        if (!membersSeed.HaveMembersBeenSeeded())
                        {
                            membersSeed.AddMembersFromJson("Data/Json/Barcelona/female-checkins2-members.data.json");
                            membersSeed.AddMembersFromJson("Data/Json/Barcelona/male-checkins2-members.data.json");
                        }
                        
                        var seeder = new CheckinSeed(_context);
                        // seeder.AddNextweekCheckinsFromJsonFile("Data/Json/Barcelona/checkins-tomorrow.json");
                        // if (seeder.SetCheckinsByLocationDatesAsToday(2, 65))
                        // {
                        //     Log.Information("Barcelona checkins updated to today");
                        // }
                        if (seeder.SetUpcomingCheckinsByLocation(8, 280, 3))
                        {
                            Log.Information("Barcelona checkins updated 3 upcoming days");
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occuried during Migration");
                    }
                }

                host.Run();
            }
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
