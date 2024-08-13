using Microsoft.AspNetCore.Identity;

namespace RegApi.Domain.Entities
{
    public class Role : IdentityRole
    {
        public string? Description { get; set; }
    }
}
