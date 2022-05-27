using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using API.Models.AuthModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public static class GenerateJwtService
{
    public static JsonResult GenerateJwtToken(ClaimsIdentity identity)
    {
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
        var response = new
        {
            access_token = encodedJwt,
            email = identity.Name
        };

        return new JsonResult(response);
    }
    
}