using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;
using TakeAwayExample2.Utility;

namespace TakeAwayExample2.Pages.Customer.Home
{
    public class IndexModel : PageModel
    {
        
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MenuItem> MenuItemList { get; set; }
        public IEnumerable<Category> CategoryList { get; set; }

        public void OnGet()
        {
            //The we casting to type claims identity hence the parenthisis
            var claimIdentity = (ClaimsIdentity) this.User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                int shoppingCartCount = _unitOfWork.ShoppingCart.GetAll(s => s.UserID == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, shoppingCartCount);
            }

            MenuItemList = _unitOfWork.MenuItem.GetAll(null, null, "Category,FoodType");

            CategoryList = _unitOfWork.Category.GetAll(null, q => q.OrderBy(c => c.DisplayOrder), null);

            //Call function to perform rounding off to 2 decimal places
            Round_OffPriceValue(MenuItemList);
        }

        void Round_OffPriceValue(IEnumerable<MenuItem> MenuItemList)
        {
            //Goes the MenuItemList and rounds of the price to 2 decimal using bankers rounding
            foreach (var menuItem in MenuItemList)
            {
                menuItem.MenuItemPrice = Decimal.Round(menuItem.MenuItemPrice, 2);
            }
        }
    }
}
