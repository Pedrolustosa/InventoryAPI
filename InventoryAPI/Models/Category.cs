using System.Text.Json.Serialization;

namespace InventoryAPI.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
