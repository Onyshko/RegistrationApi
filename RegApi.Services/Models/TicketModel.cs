using RegApi.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    public class TicketModel
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }

        public DateTime Deadline { get; set; }

        public Status Status { get; set; } = Status.InProcess;

        [Required(ErrorMessage = "User is required")]
        public required string UserId { get; set; }
    }
}
