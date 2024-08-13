using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RegApi.Domain.Entities;
using RegApi.Repository.SeedConfiguration;

namespace RegApi.Repository.Context
{
    public class DatabaseContext : IdentityDbContext<User, Role, string>
    {
        public DatabaseContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
        }
    }
}
