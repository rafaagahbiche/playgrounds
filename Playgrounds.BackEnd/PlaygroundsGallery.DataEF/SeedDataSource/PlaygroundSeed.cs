using System;
using System.Collections.Generic;
using System.Linq;
using PlaygroundsGallery.DataEF.Models;

namespace PlaygroundsGallery.DataEF.SeedDataSource
{
    public class PlaygroundSeed
    {
        private List<Playground> getPlaygrounds() =>
            new List<Playground>() {
                 new Playground {
                    Address = "Rucker Park",
                    LocationId = 7,
                    Created = DateTime.UtcNow
                },
                new Playground {
                    Address = "West 4th Street Courts",
                    LocationId = 7,
                    Created = DateTime.UtcNow
                },
                new Playground {
                    Address = "KingDome",
                    LocationId = 7,
                    Created = DateTime.UtcNow
                },
                new Playground {
                    Address = "Parc de la Barceloneta",
                    LocationId = 8,
                    Created = DateTime.UtcNow
                },
                new Playground {
                    Address = "Bir Hakeim",
                    LocationId = 9,
                    Created = DateTime.UtcNow
                },
                new Playground {
                    Address = "Canal St Martin",
                    LocationId = 9,
                    Created = DateTime.UtcNow
                },
                new Playground {
                    Address = "Metro Glaciere",
                    LocationId = 9,
                    Created = DateTime.UtcNow
                }
            };
    
        public bool Seed(GalleryContext context)
        {
            if (!context.Playgrounds.Any())
            {
                context.Playgrounds.AddRange(getPlaygrounds());
                return context.SaveChanges() > 0;
            }
        
            return false;
        }
    }
}