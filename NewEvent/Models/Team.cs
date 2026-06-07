namespace NewEvent.Models
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int JobId { get; set; }
        public Job Job { get; set; }
    }
}
