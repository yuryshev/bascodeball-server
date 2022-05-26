﻿using System.Security.Claims;
using API.Interfaces;
using API.Models.DbModels;
using API.Stores;
using Common.OperatingModels;

namespace API.Services;

public class GetIdentityService : IIdentityService
{
    private UserStore _store;

    public GetIdentityService(UserStore store)
    {
        _store = store ?? throw new ArgumentNullException(nameof(store));
    }
    
    
    public async Task<GetEntityResult<ClaimsIdentity>> GetIdentity(string inputEmail)
    {
        try
        {
            var usersResult = await _store.GetUsersAsync();
            if (!usersResult.IsSuccess)
            {
                if (usersResult.Status == GetEntityResult<List<User>>.ResultType.DatabaseError)
                {
                    return GetEntityResult<ClaimsIdentity>.FromDbError();
                } 
                if (usersResult.Status == GetEntityResult<List<User>>.ResultType.NotFound)
                {
                    return GetEntityResult<ClaimsIdentity>.FromNotFound();
                }
            }

            var user = usersResult.Entity.Find(x => x.Email == inputEmail);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return GetEntityResult<ClaimsIdentity>.FromFound(claimsIdentity);
            }
            
            return GetEntityResult<ClaimsIdentity>.FromNotFound();;
        }
        catch (Exception ex)
        {
            return GetEntityResult<ClaimsIdentity>.FromUnexpectedError();
        }
    }
    
    public async Task<GetEntityResult<ClaimsIdentity>> RegIdentity(string email, string loginName)
    {
        try
        {
            var userResult = await this._store.AddUserAsync(email, loginName);
            if (!userResult.IsSuccess)
            {
                return GetEntityResult<ClaimsIdentity>.FromDbError();
            }

            var user = userResult.Entity;
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return GetEntityResult<ClaimsIdentity>.FromFound(claimsIdentity);
        }
        catch (Exception ex)
        {
            return GetEntityResult<ClaimsIdentity>.FromUnexpectedError();
        }
    }
}