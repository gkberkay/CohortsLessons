using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Restful.API.Models
{
    public class Product
    {
        [DisplayName("Id")]
        public int Id { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Price")]
        public decimal Price { get; set; }
    }
}
