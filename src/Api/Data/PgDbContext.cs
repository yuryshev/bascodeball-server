using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class PgDbContext : DbContext
    {
        public PgDbContext(DbContextOptions<PgDbContext> options)
            : base(options)
        {
        }
        
        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
    }
}