using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models.Dto
{
    public class CityDto
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "The name length should be 3 - 50 characters.")]
        public string Name { get; set; }

        public Guid RegionId { get; set; }
    }
    public class CityUpdate : CityDto
    {
        public Guid? Id { get; set; }
    }
}
