using RegApi.Domain.Enums;

namespace RegApi.Domain.Entities
{
    public class Ticket : BaseEntity<int>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime Deadline { get; set; }
        public string UserId { get; set; }
        public Status Status { get; set; }
        public virtual User User { get; set; }
    }
}
