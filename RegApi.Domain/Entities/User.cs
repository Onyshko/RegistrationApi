﻿using Microsoft.AspNetCore.Identity;

namespace RegApi.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUri { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
