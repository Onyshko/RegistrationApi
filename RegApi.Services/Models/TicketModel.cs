using RegApi.Domain.Enums;

namespace RegApi.Services.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public Status Status { get; set; }
        public string UserId { get; set; }
    }
}
