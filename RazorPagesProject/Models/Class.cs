using System.ComponentModel.DataAnnotations;

namespace RazorPagesProject.Models
{
    public class Class
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public int PersonCount { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true; // Varsayılan olarak aktif başlat
    }
}
