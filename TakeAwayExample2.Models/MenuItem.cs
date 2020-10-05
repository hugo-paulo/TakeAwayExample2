using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TakeAwayExample2.Models
{
    [Table("MenuItems")]
    public class MenuItem
    {
        public int MenuItemID { get; set; }

        [Display(Name = "Name")]
        public string MenuItemName { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)] //will need: <textarea asp-for="Body" class="form-control" text-wrap:normal" type="text" placeholder="Please add your experience here"></textarea>
        public string MenuItemDescription { get; set; }

        [Display(Name = "Image")]
        public string MenuItemImage { get; set; }

        [Display(Name = "Price")]
        [Range(0.50, int.MaxValue, ErrorMessage = "Price should be greater than 50c")]
        public decimal MenuItemPrice { get; set; }
        //Note! when adding comma or period need add culture in startup configure (when double change db to real)

        //These are foreign keys columns to parent table
        [Display(Name = "Category Type")]
        public int CategoryID { get; set; }

        //These are the navigation properties
        [ForeignKey("CategoryID")]
        public virtual Category Category { get; set; }

        [Display(Name = "Food Type")]
        public int FoodTypeID { get; set; }

        [ForeignKey("FoodTypeID")]
        public virtual FoodType FoodType { get; set; }
    }
}
