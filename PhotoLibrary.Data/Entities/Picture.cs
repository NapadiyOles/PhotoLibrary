using System.Drawing;

namespace PhotoLibrary.Data.Entities
{
    public class Picture : BaseEntity
    {
        public string Name { get; set; }
        public string UniqueId { get; set; }
        public Image Image { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public double Rate { get; set; }
        public int RatesNumber { get; set; }
    }
}