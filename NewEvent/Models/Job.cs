namespace NewEvent.Models
{
    public class Job :BaseEntity
    {
        public string Name { get; set; }
        public List<Team> Teams { get; set; }
    }
}
