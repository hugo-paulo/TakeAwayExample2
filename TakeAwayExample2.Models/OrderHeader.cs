using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TakeAwayExample2.Models
{
    [Table("OrderHeaders")]
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderID { get; set; }

        [ForeignKey("UserID")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Order Total")]
        public decimal OrderTotal { get; set; }

        [Required]
        [Display(Name = "Collection Time")]
        public DateTime CollectionTime { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "Collection Date")]
        public DateTime CollectionDate { get; set; }

        public string Status { get; set; }

        public string PaymentStatus { get; set; }

        public string Comments { get; set; }

        [Display(Name = "Name")]
        public string PickupName { get; set; }

        [Display(Name = "Phone No.")]
        public string PhoneNumber { get; set; }

        public string TransactionID { get; set; } //on the DB is this a key? (currently not FK)
    }
}
