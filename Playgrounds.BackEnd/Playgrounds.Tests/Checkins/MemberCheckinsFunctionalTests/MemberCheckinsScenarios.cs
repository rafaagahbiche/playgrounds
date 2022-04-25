using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Checkins.Services;
using Newtonsoft.Json;
using Playgrounds.FunctionalTests;
using Xunit;

namespace MemberCheckinsFunctionalTests
{
    public class MemberCheckinsScenarios: ScenarioBase
    {
        [Fact]
        public async Task checkin_to_playground_unauthenticated_and_response_401_status_code()
        {
            // Arrange
            var req = $"/api/member-checkins/";
            var content = new StringContent(JsonConvert.SerializeObject(new CheckInForCreationDto()), UTF8Encoding.UTF8, "application/json");
            using (var server = CreateServer())
            {
                var client = server.CreateClient();

                // Act
                var response = await client.PostAsync(req, content);
                
                // Assert
                Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }
    }
}