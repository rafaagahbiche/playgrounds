using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Auth.Services;
using Newtonsoft.Json;
using Xunit;

namespace Playgrounds.FunctionalTests
{
    public class PlaygroundsScenarios : ScenarioBase
    {
        [Fact]
        public async Task Get_get_playground_by_id_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var req = Get.ItemById(2);
                var client = server.CreateClient();
                var response = await client.GetAsync(req);
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Get_get_playgrounds_by_locationId_and_response_ok_status_code()
        {
            using (var server = CreateServer())
            {
                var req = $"/api/playgrounds/locations/2";
                var client = server.CreateClient();
                var response = await client.GetAsync(req);
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task Auth_login_response_ok_response()
        {
            var authMember = new MemberToLoginDto(){
                LoginName = "Fisher",
                Password = "password"
            };
            var content = new StringContent(JsonConvert.SerializeObject(authMember), UTF8Encoding.UTF8, "application/json");
                
            using (var server = CreateServer())
            {
                var client = server.CreateClient();
                var response = await client.PostAsync(Post.Auth, content);
                var memberLoggedIn = await response.Content.ReadAsAsync<MemberLoggedInDto>();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", memberLoggedIn.Token);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
