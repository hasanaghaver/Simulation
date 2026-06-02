namespace SoftLanding.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public bool Isdeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
