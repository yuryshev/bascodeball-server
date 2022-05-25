using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Models;
using Common.OperatingModels;
using Microsoft.EntityFrameworkCore;

namespace Api.Stores
{
    public class UserStore
    {
        private readonly PgDbContext _dbContext;

        public UserStore(PgDbContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<GetEntityResult<List<string>>> GetUsersEmails()
        {
            try
            {
                var emails = this._dbContext.Users.Select(x => x.Email).ToList();
                return GetEntityResult<List<string>>.FromFound(emails);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}