using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models.Dto
{
    public class ProductDto
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "The name length should be 3 - 150 characters.")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
    }

    public class ProductUpdate : ProductDto
    {
        public Guid? Id { get; set; }
    }
}
