using Moq;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaygroundsGallery.DataEF;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;
using System.Collections.Generic;
using Xunit;
using Playgrounds.Services;

namespace Playgrounds.UnitTests
{
    public class BasicUnitTests
    {
        private readonly DbContextOptions<GalleryContext> _dbContextOptions;
        private Mock<IMapper> mockPlaygroundMapper;
        private Mock<ILogger<PlaygroundManager>> mockPlaygroundServiceLogger;

        private PlaygroundManager playgroundManager;

        public BasicUnitTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<GalleryContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryPlaygrounds")
                .Options;
            mockPlaygroundMapper = new Mock<IMapper>();
            mockPlaygroundServiceLogger = new Mock<ILogger<PlaygroundManager>>();
        }

        [Fact]
        public  async Task Get_playgrounds_by_locationId_Success()
        {
            using (var context = new GalleryContext(_dbContextOptions))
            {
                context.Playgrounds.AddRange(PlaygroundsDumbData.GetPlaygroundList());
                context.SaveChanges();
                playgroundManager = new PlaygroundManager(
                    context, 
                    mockPlaygroundMapper.Object, 
                    mockPlaygroundServiceLogger.Object);
                
                    mockPlaygroundMapper.Setup(mapper => mapper.Map<IEnumerable<PlaygroundDto>>(It.IsAny<IEnumerable<Playground>>()))
                    .Returns((IEnumerable<Playground> playgroundEntities) => {
                        var playgroundDtos = new List<PlaygroundDto>();
                        foreach (Playground playgroundEntity in playgroundEntities)
                        {
                            playgroundDtos.Add(new PlaygroundDto(){
                                Id = playgroundEntity.Id,
                                Address = playgroundEntity.Address,
                                LocationStr = "Main street"
                            });
                        }
                        
                        return playgroundDtos;
                                    
                });

                // Act
                var res = await playgroundManager.GetAllPlaygroundsByLocationIdAsync(1);

                // Assert
                Assert.NotEmpty(res);
            }
        }
    }
}