﻿using GoogleAuthorization.Services;
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
        public string GetAuthorizationCodeEndpoint()
        {
            return _googleAuthorizationService.GetAuthorizationCodeEndpoint();
        }


        [HttpGet("token")]
        public async Task<string> GetAuthorizationToken(string code)
        {
            return await _googleAuthorizationService.GetAuthorizationToken(code);
        }
    }
}