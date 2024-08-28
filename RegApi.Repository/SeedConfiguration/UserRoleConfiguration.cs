using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RegApi.Repository.SeedConfiguration
{
    /// <summary>
    /// Configures the <see cref="IdentityUserRole{string}"/> entity with initial role-user data.
    /// </summary>
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        /// <summary>
        /// Configures the entity with specific settings for the <see cref="IdentityUserRole{string}"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = "16494060-c41a-4343-9128-388ed5e61d77",
                    RoleId = "11a3d48f-e7ff-46e6-a897-9664e603a824"
                }
            );
        }
    }
}
