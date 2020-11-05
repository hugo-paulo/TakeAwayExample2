using System;
using System.Collections.Generic;
using System.Text;

namespace TakeAwayExample2.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public OrderDetail OrderDetail { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
