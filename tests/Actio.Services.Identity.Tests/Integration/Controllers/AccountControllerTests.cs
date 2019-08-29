using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Actio.Common.Auth;
using FluentAssertions;

namespace Actio.Services.Identity.Tests.Integration.Controllers
{
    public class AccountControllerTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        
        public AccountControllerTests()
        {
            _server = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task account_controller_login_should_return_json_web_token()
        {
            var payload = GetPayload(new {email = "user1@email.com", password = "secret"});
            var response = await _client.PostAsync("login", payload);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var jwt = JsonConvert.DeserializeObject<JsonWebToken>(content);

            jwt.Should().NotBeNull();
            jwt.Token.Should().NotBeEmpty();
            jwt.Expires.Should().BeGreaterThan(0);
        }

        protected static StringContent GetPayload(object data)
        {
            var json = JsonConvert.SerializeObject(data);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}