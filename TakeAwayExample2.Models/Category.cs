using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TakeAwayExample2.Models
{
    [Table("Categories")]
    public class Category
    {
        //Tells EF this is a PK
        //[Key]
        public int CategoryID { get; set; }
        
        [Required]
        [Display(Name = "Category Name")]
        [MaxLength(100)]
        public string CategoryName { get; set; }

        [Required]
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
    }
}
