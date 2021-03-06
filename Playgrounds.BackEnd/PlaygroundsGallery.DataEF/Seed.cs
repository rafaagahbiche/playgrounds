using System;
using System.Linq;
using PlaygroundsGallery.Domain.Models;
namespace PlaygroundsGallery.DataEF
{
    public class Seed
    {
        public static void SeedLocations(GalleryContext context)
        {
            if (!context.Locations.Any())
            {
                context.Locations.Add(
                   new Location
                   {
                       City = "New York City",
                       Country = "US",
                       Created = DateTime.UtcNow
                   });
                context.Locations.Add(
                    new Location
                    {
                        City = "Barcelona",
                        Country = "Spain",
                       Created = DateTime.UtcNow
                    });
                context.Locations.Add(
                    new Location
                    {
                        City = "Paris",
                        Country = "France",
                       Created = DateTime.UtcNow
                    });
                context.Locations.Add(
                    new Location
                    {
                        City = "Berlin",
                        Country = "Germany",
                       Created = DateTime.UtcNow
                    });
                
                context.Locations.Add(
                   new Location
                   {
                       City = "Toronto",
                       Country = "Canada",
                       Created = DateTime.UtcNow
                   });
                context.Locations.Add(
                   new Location
                   {
                       City = "London",
                       Country = "UK",
                       Created = DateTime.UtcNow
                   });
            }
        }

        public static void SeedPlaygrounds(GalleryContext context)
        {
            if (!context.Playgrounds.Any())
            {
                context.Playgrounds.Add(
                    new Playground {
                        Address = "Rucker Park",
                        LocationId = 1,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "West 4th Street Courts",
                        LocationId = 1,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "KingDome",
                        LocationId = 1,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "Parc de la Barceloneta",
                        LocationId = 2,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "Carrer Marina",
                        LocationId = 2,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "Bir Hakeim",
                        LocationId = 3,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "Canal St Martin",
                        LocationId = 3,
                       Created = DateTime.UtcNow
                    }
                );
                context.Playgrounds.Add(
                    new Playground {
                        Address = "Metro Glaciere",
                        LocationId = 3,
                       Created = DateTime.UtcNow
                    }
                );
            }
        }

        public static void SeedProfilePictures(GalleryContext context)
        {
            if (!context.ProfilePictures.Any())
            {
                context.Add(new ProfilePicture 
                {
                    MemberId = 2,
                    Url = "https://randomuser.me/api/portraits/men/76.jpg"
                });
                context.Add(new ProfilePicture 
                {
                    MemberId = 1012,
                    Url = "https://randomuser.me/api/portraits/men/14.jpg"
                });
                context.Add(new ProfilePicture 
                {
                    MemberId = 1019,
                    Url = "https://randomuser.me/api/portraits/men/4.jpg"
                });
                context.Add(new ProfilePicture 
                {
                    MemberId = 1029,
                    Url = "https://randomuser.me/api/portraits/women/49.jpg"
                });
            }
        }

        public static void SaveSeeds(GalleryContext context)
        {
            context.SaveChanges();
        }
    }
}