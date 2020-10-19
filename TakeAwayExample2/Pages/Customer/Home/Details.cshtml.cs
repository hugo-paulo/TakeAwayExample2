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
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public DetailsModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [BindProperty]
        public ShoppingCart ShoppingCartObj { get; set; }

        //We get the id parameter(menuItemID) from the tag helper from the index page from each card details link
        public void OnGet(int id)
        {
            ShoppingCartObj = new ShoppingCart()
            {
                MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includedProperties: "Category,FoodType", filter: m => m.MenuItemID == id),
                MenuItemID = id
            };

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                ShoppingCartObj.UserID = claim.Value;

                ShoppingCart obj = _unitOfWork.ShoppingCart.GetFirstOrDefault(s => s.UserID == ShoppingCartObj.UserID && s.MenuItemID == ShoppingCartObj.MenuItemID);

                //If the shopping cart is empty add the selected items to it
                if (obj == null)
                {
                    _unitOfWork.ShoppingCart.AddItem(ShoppingCartObj);
                }
                else
                {
                    //adding additional items to the shopping cart
                    _unitOfWork.ShoppingCart.IncreamentCount(obj, ShoppingCartObj.ItemCount);
                }

                _unitOfWork.Save();

                //update the session
                var shoppingCartCount = _unitOfWork.ShoppingCart.GetAll(s => s.UserID == ShoppingCartObj.UserID).ToList().Count;
                HttpContext.Session.SetInt32(SD.ShoppingCart, shoppingCartCount);

                return RedirectToPage("Index");
            }
            else
            {
                //Needed even though count is invalid the page needs the shopping cart object menuItem when page is re rendered.
                ShoppingCartObj.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(includedProperties: "Category,FoodType", filter: m => m.MenuItemID == ShoppingCartObj.MenuItemID);
                return Page();
            }
        }
    }
}
