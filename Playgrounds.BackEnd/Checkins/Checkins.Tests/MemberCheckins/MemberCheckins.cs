using Checkins.Services;
using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;

namespace Tests
{
    public class MemberCheckins
    {
        private CheckinMember checkinMemberManager;

        [SetUp]
        public void Setup()
        {
            Tests.SetupCheckinRepo();
            Tests.SetupCheckinMapper();
            checkinMemberManager = new CheckinMember(Tests.MockCheckinRepository.Object, Tests.MockCheckinMapper.Object);
        }

        [Test]
        public async Task CheckinToPlaygroundAsync_Success()
        {
            var checkin = new CheckInForCreationDto()
            {
                PlaygroundId = 1,
                MemberId = 1,
                CheckInDate = DateTime.Now
            };

            var checkinToReturnDto = await checkinMemberManager.CheckinToPlaygroundAsync(checkin);
            Assert.NotNull(checkinToReturnDto);
            Assert.AreEqual(checkinToReturnDto.PlaygroundAddress, "new playground");
        }
    }
}