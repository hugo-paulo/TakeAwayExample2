using System;
using System.Collections.Generic;
using System.Text;

namespace TakeAwayExample2.Models.ViewModels
{
    public class ShoppingCartOrdersVM
    {
        public List<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
