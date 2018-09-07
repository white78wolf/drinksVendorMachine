using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace DrinksVendorMachine.Models
{
    public class Drink
    {
        public int DrinkId { get; set; }

        [Required]
        [AllowHtml]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, 210)] // условное ограничение цены
        public int Price { get; set; }

        [Required]
        [Range(0, 180)] // условное ограничение количества напитков в "автомате"
        public int Quantity { get; set; }

        [Column(TypeName = "image")]
        public byte[] ImageData { get; set; }

        public string ImageMimeType { get; set; }

        public Drink()
        {
          
        }

        public Drink(int id, string name, int price, int quantity, byte[] imageData, string imageMimeType)
        {
            this.DrinkId = id;
            this.Name = name;
            this.Price = price;
            this.Quantity = quantity;
            this.ImageData = imageData;
            this.ImageMimeType = imageMimeType;
        }
    }
}