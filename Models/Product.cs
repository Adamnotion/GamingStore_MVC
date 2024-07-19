using System.ComponentModel.DataAnnotations;

namespace GamingStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(100)]
        public string Brand { get; set; } = "";

        [MaxLength(100)]
        public string Category { get; set; } = "";

        public decimal Price { get; set; }

        [MaxLength(100)]
        public string Description { get; set; } = "";

        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}