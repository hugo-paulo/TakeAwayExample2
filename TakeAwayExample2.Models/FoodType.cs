using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TakeAwayExample2.Models
{
    [Table("FoodTypes")]
    public class FoodType
    {
        //[Key]
        public int FoodTypeID { get; set; }

        [Required]
        [Display(Name = "Food Type Name")]
        [MaxLength(100)]
        public string FoodTypeName { get; set; }

    }
}
