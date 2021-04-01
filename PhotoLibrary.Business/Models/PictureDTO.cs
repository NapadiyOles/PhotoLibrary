using System.Drawing;

namespace PhotoLibrary.Business.Models
{
    public class PictureDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UniqueId { get; set; }
        public Image Image { get; set; }
        public string UserId { get; set; }
        public double Rate { get; set; }
        public int RatesNumber { get; set; }
    }
}