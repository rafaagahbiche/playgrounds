using Checkins.Services;
using NUnit.Framework;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlaygroundsGallery.DataEF;
using Microsoft.Extensions.Logging;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;
using System.Collections.Generic;

namespace Checkins.Tests
{
    public class BasicUnitTests
    {
        private DbContextOptions<GalleryContext> _dbContextOptions;
        private Mock<IMapper> mockCheckinMapper;

        [SetUp]
        public void Setup()
        {
             _dbContextOptions = new DbContextOptionsBuilder<GalleryContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryPlaygrounds")
                .Options;
            mockCheckinMapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Get_Checkins_by_playgroundId_and_date_async_Success()
        {
            using (var context = new GalleryContext(_dbContextOptions))
            {
                context.CheckIns.AddRange(CheckinsDumbData.GetCheckinsList());
                context.SaveChanges();
                var checkinServiceLogger = new Mock<ILogger<PlaygroundCheckinService>>();
                var checkinService = new PlaygroundCheckinService(
                    context, 
                    mockCheckinMapper.Object,
                    checkinServiceLogger.Object);

                mockCheckinMapper.Setup(mapper => mapper.Map<IEnumerable<CheckinDto>>(It.IsAny<IEnumerable<CheckIn>>()))
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
                
                // Act
                var res = await checkinService.GetCheckinsAtPlaygroundByDateAsync(2, DateTime.Today);

                // Assert    
                Assert.IsNotEmpty(res);
            }
        }

        [Test]
        public async Task CheckinToPlaygroundAsync_Success()
        {
            var mockLogger = new Mock<ILogger<MemberCheckinService>>();
            MemberCheckinService checkinMemberManager;
            using (var context = new GalleryContext(_dbContextOptions))
            {
                context.CheckIns.AddRange(CheckinsDumbData.GetCheckinsList());
                context.SaveChanges();
                checkinMemberManager = new MemberCheckinService(
                    context, 
                    mockCheckinMapper.Object,
                    mockLogger.Object);
            }

            mockCheckinMapper.Setup(mapper => mapper.Map<CheckinDto>(It.IsAny<CheckIn>())).Returns((CheckIn checkinEntity) => {
                return new CheckinDto(){
                    MemberLoginName = "new hooper in town",
                    CheckInDate = checkinEntity.CheckInDate
                };
            });
            var checkin = new CheckInForCreationDto()
            {
                PlaygroundId = 1,
                MemberId = 1,
                CheckInDate = DateTime.Now
            };

            var checkinToReturnDto = await checkinMemberManager.GetCheckInById(1);
            Assert.NotNull(checkinToReturnDto);
        }

         [Test]
        public async Task CheckinsToTimeslotsAsync_Success()
        {
            var mockLogger = new Mock<ILogger<PlaygroundCheckinService>>();
            PlaygroundCheckinService playgroundCheckinService;
            mockCheckinMapper.Setup(mapper => mapper.Map<CheckinDto>(It.IsAny<CheckIn>())).Returns((CheckIn checkinEntity) => {
                return new CheckinDto(){
                    Id = checkinEntity.Id,
                    MemberLoginName = "new hooper in town",
                    CheckInDate = checkinEntity.CheckInDate
                };
            });
            using (var context = new GalleryContext(_dbContextOptions))
            {
                context.CheckIns.Add(new CheckIn(){
                    Id = 1,
                    CheckInDate = DateTime.Today,
                    Member = new Member(){
                        Id = 33,
                        ProfilePictures = new List<ProfilePicture>()
                    },
                    MemberId = 33,
                    PlaygroundId = 2
                });
                context.CheckIns.Add(new CheckIn(){
                    Id = 2,
                    CheckInDate = DateTime.Today.AddHours(1),
                    Member = new Member(){
                        Id = 11,
                        ProfilePictures = new List<ProfilePicture>()
                    },
                    MemberId = 11,
                    PlaygroundId = 2
                });
                context.SaveChanges();
                playgroundCheckinService = new PlaygroundCheckinService(
                    context, 
                    mockCheckinMapper.Object,
                    mockLogger.Object);

                // Act
                var timeslots = await playgroundCheckinService.GetTimeSlotsAtPlaygroundByDateAsync(2, DateTime.Today);
                
                // Assert
                Assert.IsNotNull(timeslots);
                Assert.IsNotEmpty(timeslots);
                Assert.AreEqual(2, timeslots.Count());
            }
        }
    }
}