using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Seed;
namespace PlaygroundsGallery.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var host = CreateWebHostBuilder(args).Build();
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment != EnvironmentName.Development)
                {

                }
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

                        // Set 7 checkins today for Playground 16
                        var checkinsUpdated2 =  CheckinUpdate.UpdateCheckinsByPlaygroundId(context, 16, 7);

                        if (locationsSeed || playgroundsSeed || profilePicturesSeed || checkinsUpdated1 || checkinsUpdated2)
                        {
                            InitialSeed.SaveSeeds(context);
                        } 

                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occuried during Migration");
                    }
                }

                host.Run();
            }
            catch (Exception)
            {
            }    
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
