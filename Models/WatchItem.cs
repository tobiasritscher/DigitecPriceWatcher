using System.ComponentModel.DataAnnotations;

namespace RazorPagesWatchItem.Models
{
    public class WatchItem
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public Availability isAvailable { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ItemUrl { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"Name: {Name}, Availability: {isAvailable}, Image: {Category}, Price: {Price}";
        }
    }

    

    public enum Availability
    {
        inStock,
        ordered,
        outOfStock
    }
}