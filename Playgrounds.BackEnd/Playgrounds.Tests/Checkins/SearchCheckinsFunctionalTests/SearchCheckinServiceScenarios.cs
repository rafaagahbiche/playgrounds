using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Checkins.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Playgrounds.FunctionalTests;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Models;
using Xunit;

namespace SearchCheckinsFunctionalTests
{
    public class SearchCheckinServiceScenarios : ScenarioBase
    {
        private Mock<IMapper> mockMapper;

        private void setupMapperCheckinToCheckinDto()
        {
            mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<CheckinDto>>(It.IsAny<IEnumerable<CheckIn>>()))
                                .Returns((IEnumerable<CheckIn> checkinEntities) => {
                                    var checkinDtos = new List<CheckinDto>();
                                    foreach (CheckIn checkinEntity in checkinEntities)
                                    {
                                        checkinDtos.Add(new CheckinDto(){
                                            Id = checkinEntity.Id,
                                            CheckInDate = checkinEntity.CheckInDate,
                                            MemberLoginName = "member",
                                            MemberProfilePictureUrl = "http://myprofile.pic"
                                        });
                                    }
                                    return checkinDtos;
                });
                mockMapper.Setup(mapper => mapper.Map<CheckinDto>(It.IsAny<CheckIn>()))
                            .Returns((CheckIn checkinEntity) => {
                                    return new CheckinDto(){
                                        Id = checkinEntity.Id,
                                        CheckInDate = checkinEntity.CheckInDate,
                                        MemberLoginName = "member",
                                        MemberProfilePictureUrl = "http://myprofile.pic"
                                };
                });
        }

        private async Task<HttpResponseMessage> snapUpServerAndSendSearchRequest()
        {
            using (var server = CreateServer())
            {
                var client = server.CreateClient();
                var date = DateTime.Today.ToString("yyyy-MM-dd");
                return await client.GetAsync($"/api/checkins/locations/2/timeslots/{date}");
            }
        }

        [Fact]
        public async Task Get_checkins_by_playgroundId_between_date_success()
        {
            using (var server = CreateServer())
            {
                // Arrange
                var context = server.Host.Services.GetRequiredService<GalleryContext>();
                setupMapperCheckinToCheckinDto();
                var mockLogger = new Mock<ILogger<PlaygroundCheckinService>>();
                var checkinService = new PlaygroundCheckinService(context, mockMapper.Object, mockLogger.Object);

                // Act
                var result = await checkinService.GetCheckinsAtPlaygroundBetweenTwoDatesAsync(16, DateTime.Today.AddDays(-1), DateTime.Today);

                // Assert
                Assert.NotEmpty(result);
            }
        }

        [Fact]
        public async Task Get_checkins_by_locationId_after_date_success()
        {
            using (var server = CreateServer())
            {
                // Arrange
                var context = server.Host.Services.GetRequiredService<GalleryContext>();
                setupMapperCheckinToCheckinDto();
                var mockLogger = new Mock<ILogger<LocationCheckinService>>();
                var checkinService = new LocationCheckinService(context, mockMapper.Object, mockLogger.Object);

                // Act
                var timeslots = await checkinService.GetTimeSlotsAtLocationByDateAsync(2, DateTime.Today.AddDays(1));
                
                // Assert
                Assert.Equal(70, timeslots.Count());
            }
        }

        [Fact]
        public async Task comapare_concurrent_servers_req_checkins_by_locationId()
        {
            var res1 = await snapUpServerAndSendSearchRequest();
            var res2 = await snapUpServerAndSendSearchRequest();
            res1.EnsureSuccessStatusCode();
            res2.EnsureSuccessStatusCode();
        }
    }
}
