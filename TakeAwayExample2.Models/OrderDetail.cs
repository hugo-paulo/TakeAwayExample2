using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TakeAwayExample2.Models
{
    [Table("OrderDetails")]
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [Required]
        public int OrderHeaderID { get; set; }

        [Required]
        public int MenuItemID { get; set; }

        public int OrderCount { get; set; }
        public string OrderName { get; set; }
        
        //May not be needed because have description of menu item, what would describe the order?
        public string OrderDescription { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("OrderHeaderID")]
        public virtual OrderHeader OrderHeader { get; set; }

        [ForeignKey("MenuItemID")]
        public virtual MenuItem MenuItem { get; set; }

    }
}
