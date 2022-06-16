using GoogleAuthorization.Dtos;
using GoogleAuthorization.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthorization.Controllers
{
    [Route("api/google/authorization")]
    [ApiController]
    public class GoogleAuthorizationController : ControllerBase
    {
        private readonly GoogleAuthorizationService _googleAuthorizationService;

        public GoogleAuthorizationController(GoogleAuthorizationService googleAuthorizationService)
        {
            _googleAuthorizationService = googleAuthorizationService;
        }

        [HttpGet("code")]
        public void GetAuthorizationCodeEndpoint()
        {
            string url = _googleAuthorizationService.GetAuthorizationCodeEndpoint();
            HttpContext.Response.Redirect(url);
        }


        [HttpGet("token")]
        public async Task GetAuthorizationToken(string code)
        {
            AccessTokenPayloadDto dto = await _googleAuthorizationService.GetAuthorizationToken(code);
            string url = await _googleAuthorizationService.ProvideJWTUrl(dto);
            HttpContext.Response.Redirect(url);
        }

    }
}
