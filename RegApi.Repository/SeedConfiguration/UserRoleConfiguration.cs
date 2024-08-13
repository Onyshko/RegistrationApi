using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RegApi.Repository.SeedConfiguration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "aaee42b9-aa16-40cb-bee4-1c51d41e3e4a",
                    RoleId = "11a3d48f-e7ff-46e6-a897-9664e603a824"
                }
            );
        }
    }
}
