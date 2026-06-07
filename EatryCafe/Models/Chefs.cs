using EatryCafe.Models.Base;

namespace EatryCafe.Models
{
    public class Chefs : BaseEntity
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Job { get; set; }
        public string Description { get; set; }
    }
}
