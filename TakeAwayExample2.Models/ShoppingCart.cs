using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TakeAwayExample2.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            ItemCount = 1;
        }

        public int ShoppingCartID { get; set; }
        public int MenuItemID { get; set; }
        public string UserID { get; set; }
        [Range(1, 100, ErrorMessage = "Sorry we cannot process more than 100 items")]
        public int ItemCount { get; set; }

        //Navigation properties
        
        [NotMapped]
        [ForeignKey("MenuItemID")]
        public virtual MenuItem MenuItem { get; set; }

        [NotMapped]
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
    }
}
