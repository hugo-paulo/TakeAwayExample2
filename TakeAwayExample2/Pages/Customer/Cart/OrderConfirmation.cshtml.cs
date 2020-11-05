using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TakeAwayExample2.Pages.Customer.Cart
{
    public class OrderConfirmationModel : PageModel
    {
        //This must match the FK (OrderDetailID) of the OrderDetail table.
        [BindProperty]
        public int OrderDetailID { get; set; }

        public void OnGet(int id)
        {
            OrderDetailID = id;
        }
    }
}
