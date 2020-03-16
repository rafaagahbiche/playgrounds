using AutoMapper;
using Moq;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF.Repositories;
using PlaygroundsGallery.DataEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Checkins.Services;

namespace Tests
{
    public class Tests
    {
        public static Mock<IRepository<CheckIn>> MockCheckinRepository;
        public static Mock<IMapper> MockCheckinMapper;
        public static Mock<ILogger> MockLogger;
        private static ICollection<CheckIn> checkinsCollection;
        public static void SetupCheckinRepo()
        {
            MockCheckinRepository = new Mock<IRepository<CheckIn>>();
            checkinsCollection = new List<CheckIn>();
            MockCheckinRepository.Setup(repo => repo.Add(It.IsAny<CheckIn>())).ReturnsAsync((CheckIn newCheckin) => {
                newCheckin.Id = 10;
                checkinsCollection.Add(newCheckin);
                return true;
            });
            MockCheckinRepository.Setup(repo => repo.Remove(It.IsAny<CheckIn>())).ReturnsAsync((CheckIn checkinToRemove) => {
                return checkinsCollection.Remove((CheckIn)checkinToRemove);
            });
            MockCheckinRepository.Setup(repo => repo.Get(It.IsAny<int>())).ReturnsAsync((int checkinId) => {
                return checkinsCollection.Where(checkin => checkin.Id == checkinId).FirstOrDefault();
            });
        }

        public static void SetupLogging()
        {
            MockLogger = new Mock<ILogger>();
            MockLogger.Setup(log => log.LogError(It.IsAny<Exception>(), It.IsAny<string>()));
        }
        public static void SetupCheckinMapper()
        {
            MockCheckinMapper = new Mock<IMapper>();
            MockCheckinMapper.Setup(mapper => mapper.Map<CheckIn>(It.IsAny<CheckInForCreationDto>())).Returns((CheckInForCreationDto checkinForCreation) => {
                return new CheckIn(){
                    MemberId = checkinForCreation.MemberId,
                    PlaygroundId = checkinForCreation.PlaygroundId,
                    CheckInDate = checkinForCreation.CheckInDate
                };
            });
            MockCheckinMapper.Setup(mapper => mapper.Map<CheckinDto>(It.IsAny<CheckIn>())).Returns((CheckIn checkinEntity) => {
                return new CheckinDto(){
                    MemberLoginName = "new hooper in town",
                    PlaygroundAddress = "new playground", 
                    CheckInDate = checkinEntity.CheckInDate
                };
            });
        }
    }
}