using System;
using System.Collections.Generic;
using System.Linq;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.SeedDataSource
{
    public class LocationSeed
    {
        private List<Location> getLocations() =>
            new List<Location>(){
                new Location {
                    City = "New York City",
                    Country = "US",
                    Created = DateTime.UtcNow
                },
                new Location
                {
                    City = "Barcelona",
                    Country = "Spain",
                    Created = DateTime.UtcNow
                },
                new Location
                {
                    City = "Paris",
                    Country = "France",
                    Created = DateTime.UtcNow
                },
                new Location
                {
                    City = "Berlin",
                    Country = "Germany",
                    Created = DateTime.UtcNow
                },
                new Location
                {
                    City = "Toronto",
                    Country = "Canada",
                    Created = DateTime.UtcNow
                },
                new Location
                {
                    City = "London",
                    Country = "UK",
                    Created = DateTime.UtcNow
                }
            };
        
        public bool Seed(GalleryContext context)
        {
            if (!context.Locations.Any())
            {
                context.Locations.AddRange(getLocations());
                return context.SaveChanges() > 0;
            }
        
            return false;
        }
    }
}