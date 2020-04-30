using System.Collections.Generic;
using PlaygroundsGallery.DataEF.Models;

namespace Playgrounds.UnitTests
{
    public static class PlaygroundsDumbData
    {
        public static List<Playground> GetPlaygroundList()
        {
            var playgrounds = new List<Playground>();
            playgrounds.Add(new Playground(){
                Id = 1,
                LocationId = 1,
                Address = "playground1"
            });
            playgrounds.Add(new Playground(){
                Id = 2,
                LocationId = 1,
                Address = "playground2"
            });
            playgrounds.Add(new Playground(){
                Id = 3,
                LocationId = 1,
                Address = "playground3"
            });
            playgrounds.Add(new Playground(){
                Id = 4,
                LocationId = 1,
                Address = "playground4"
            });

            return playgrounds;
        }
    }
}