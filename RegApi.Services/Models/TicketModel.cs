using RegApi.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace RegApi.Services.Models
{
    /// <summary>
    /// Represents a model for a ticket in the system.
    /// </summary>
    public class TicketModel
    {
        /// <summary>
        /// Gets or sets the unique identifier for the ticket.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the ticket.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the ticket.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the deadline for the ticket.
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets the current status of the ticket.
        /// Default value is <see cref="Status.InProcess"/>.
        /// </summary>
        public Status Status { get; set; } = Status.InProcess;

        /// <summary>
        /// Gets or sets the identifier of the user associated with the ticket.
        /// </summary>
        [Required(ErrorMessage = "User is required")]
        public required string UserId { get; set; }
    }

}
