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
        public static void Main(string[] args)
        {
            try
            {
                var config = GetConfiguration();
                Log.Logger = CreateSerilogLogger(config);
                var host = CreateWebHostBuilder(args).Build();
                // Uncomment to create new database
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<GalleryContext>();
                        context.CurrentDatabaseMigrate();
                        var locationsSeed = InitialSeed.SeedLocations(context);
                        var playgroundsSeed = InitialSeed.SeedPlaygrounds(context);
                        var profilePicturesSeed = InitialSeed.SeedProfilePictures(context);
                        // Add new members with checkins to playground;
                        // var membersSeed = new MembersSeedFromJsonData(context);
                        // membersSeed.AddMembersFromJson("Data/Json/Barcelona/male-checkins2-members.data.json");
                        // membersSeed.AddMembersFromJson("Data/Json/Barcelona/female-checkins2-members.data.json");

                        // Set 6 checkins today for Playground 15
                        var checkinsUpdated1 = CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 15, 6);
                        var checkinsUpdated3 = CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 2, 10);
                        var checkinsUpdated4 = CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 3, 10);
                        var checkinsUpdated5 = CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 4, 10);
                        var checkinsUpdated6 = CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 5, 10);

                        // Set 7 checkins today for Playground 16
                        var checkinsUpdated7 =  CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 16, 7);
                        var checkinsUpdated2 =  CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 17, 7);

                        if (locationsSeed || playgroundsSeed || profilePicturesSeed 
                        || checkinsUpdated1 
                        || checkinsUpdated3 
                        || checkinsUpdated4 
                        || checkinsUpdated5 
                        || checkinsUpdated6 
                        || checkinsUpdated7 
                        || checkinsUpdated2) {
                            Log.Information("data base updated at start");
                            InitialSeed.SaveSeeds(context);
                        } 
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occuried during Migration");
                    }
                }

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
            }    
        }
        
        private static Serilog.ILogger CreateSerilogLogger(IConfiguration configuration)
        {
            var logstashUrl = configuration["Serilog:LogstashgUrl"];
            return new LoggerConfiguration()
                .MinimumLevel.Debug()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();
        }

        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            var config = builder.Build();

            return builder.Build();
        }
        
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();
        }
    }
}
