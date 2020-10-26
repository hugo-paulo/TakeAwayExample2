using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;
using TakeAwayExample2.Models.ViewModels;

namespace TakeAwayExample2.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ShoppingCartOrdersVM detailCart { get; set; }

        public IActionResult OnGet()
        {
            detailCart = new ShoppingCartOrdersVM()
            {
                OrderHeader = new OrderHeader()
            };

            detailCart.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity) User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(c => c.UserID == claim.Value);

            if (cart != null)
            {
                detailCart.ShoppingCartList = cart.ToList();
            }

            foreach (var item in detailCart.ShoppingCartList)
            {
                item.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(i => i.MenuItemID == item.MenuItemID);
                detailCart.OrderHeader.OrderTotal += (item.MenuItem.MenuItemPrice * item.ItemCount);
            }

            User user = _unitOfWork.User.GetFirstOrDefault(u => u.Id == claim.Value);

            detailCart.OrderHeader.PickupName = user.FullName;
            detailCart.OrderHeader.CollectionTime = DateTime.Now;
            detailCart.OrderHeader.PickupPhoneNumer = user.PhoneNumber;

            return Page();
        }
    }
}
