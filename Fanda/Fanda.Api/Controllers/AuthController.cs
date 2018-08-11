using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace Fanda.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> ClientToken(string clientId, string clientSecret)
        {
            string apiUrl = _configuration["AppSettings:ApiUrl"];

            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync(apiUrl);
            if (disco.IsError)
            {
                return BadRequest(disco.Error);
            }

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(IdentityServerConfig.FandaApiName);

            //var tokenClient = new TokenClient(disco.TokenEndpoint, "fanda.users", "aad4fa2b-9c08-4e53-8af3-9ff8d8c2892f");
            //var tokenResponse = tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "fanda-api").Result;

            if (tokenResponse.IsError)
            {
                //Console.WriteLine(tokenResponse.Error);
                return BadRequest(tokenResponse.Error);
            }
            return Ok(tokenResponse.Json);
        }

        [HttpGet]
        public async Task<IActionResult> UserToken(string userName, string password)
        {
            string apiUrl = _configuration["AppSettings:ApiUrl"];
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync(apiUrl);
            if (disco.IsError)
            {
                return BadRequest(disco.Error);
            }

            // request token
            //var tokenClient = new TokenClient(disco.TokenEndpoint, clientId, clientSecret);
            //var tokenResponse = tokenClient.RequestClientCredentialsAsync(IdentityServerConfig.FandaApiName).Result;

            var tokenClient = new TokenClient(disco.TokenEndpoint, IdentityServerConfig.FandaUsersClientId, IdentityServerConfig.FandaUsersSecret);
            var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync(userName, password, IdentityServerConfig.FandaApiName);

            if (tokenResponse.IsError)
            {
                //Console.WriteLine(tokenResponse.Error);
                return BadRequest(tokenResponse.Error);
            }
            return Ok(tokenResponse.Json);
        }
    }
}