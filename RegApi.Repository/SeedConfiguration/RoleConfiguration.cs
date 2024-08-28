using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RegApi.Domain.Entities;
using RegApi.Repository.Constants;

namespace RegApi.Repository.SeedConfiguration
{
    /// <summary>
    /// Configures the entity type for the <see cref="Role"/> entity.
    /// </summary>
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        /// <summary>
        /// Configures the entity with specific settings for the <see cref="Role"/> entity.
        /// </summary>
        /// <param name="builder">The builder used to configure the entity type.</param>
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = "f99d2eae-0f12-4873-88ed-4df42a9a4cda",
                    Name = RoleNames.Visitor,
                    NormalizedName = "VISITOR",
                    Description = "The visitor role for the user",
                },
                new Role
                {
                    Id = "11a3d48f-e7ff-46e6-a897-9664e603a824",
                    Name = RoleNames.Admin,
                    NormalizedName = "ADMIN",
                    Description = "The admin role for the user",
                }
            );
        }
    }
}
