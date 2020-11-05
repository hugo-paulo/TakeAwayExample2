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
using TakeAwayExample2.Models.ViewModels;
using TakeAwayExample2.Utility;

namespace TakeAwayExample2.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Creates an association with the view model
        public ShoppingCartOrdersVM shoppingCartOrdersVM { get; set; }

        public void OnGet()
        {
            //Initialise the view model
            shoppingCartOrdersVM = new ShoppingCartOrdersVM()
            {
                OrderHeader = new Models.OrderHeader()
            };

            //Start the order total with 0, no item have been added
            shoppingCartOrdersVM.OrderHeader.OrderTotal = 0;

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //Creates a drop down list for all items user added to shopping cart
            IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.GetAll(c => c.UserID == claim.Value);

            if (cart != null)
            {
                shoppingCartOrdersVM.ShoppingCartList = cart.ToList();
            }

            foreach (var item in shoppingCartOrdersVM.ShoppingCartList)
            {
                item.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(m => m.MenuItemID == item.MenuItemID);

                Round_OffPriceValue(item.MenuItem);
                //shoppingCartOrdersVM.OrderHeader.OrderTotal += (item.MenuItem.MenuItemPrice * item.ItemCount);
                shoppingCartOrdersVM.OrderHeader.OrderTotal += (item.MenuItem.MenuItemPrice * item.ItemCount);

            }
            
        }

        //The prefix OnPost or onGet are the http methods, the appended name will match the url.
        //Here the plus matches the value of the asp-page-handler
        //The cartId must match the 3rd word of the asp-route-cartId
        public IActionResult OnPostPlus(int cartId)
        {
            //var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ShoppingCartID == cartId);
            var cart = GetShoppingCart(cartId);

            _unitOfWork.ShoppingCart.IncreamentCount(cart, 1);
            _unitOfWork.Save();

            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            //var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ShoppingCartID == cartId);
            var cart = GetShoppingCart(cartId);

            //When decrement to 1 then we have to delete the item and not just decrement the count
            if (cart.ItemCount == 1)
            {
                /*Original Version*/
                //_unitOfWork.ShoppingCart.Remove(cartId);
                //_unitOfWork.Save();

                //var cartIconCount = _unitOfWork.ShoppingCart.GetAll(u => u.UserID == cart.UserID).ToList().Count();
                //HttpContext.Session.SetInt32(SD.ShoppingCart, cartIconCount);

                /*Alternative version*/
                DeleteItemFromCart(cart);
                UpdateSession(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.DecreamentCount(cart, 1);
                _unitOfWork.Save();
            }

            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int cartId)
        {
            /*Original*/
            //var cart = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ShoppingCartID == cartId);
            /*Alternate*/
            var cart = GetShoppingCart(cartId);

            /*Original*/
            //_unitOfWork.ShoppingCart.DeleteItem(cart);
            //_unitOfWork.Save();
            /*Alternate*/
            DeleteItemFromCart(cart);

            UpdateSession(cart);

            return RedirectToPage("/Customer/Cart/Index");
        }

        //An Alternate to search for the shopping cart in each of the above functions
        ShoppingCart GetShoppingCart(int cartId)
        {
            var sc = _unitOfWork.ShoppingCart.GetFirstOrDefault(c => c.ShoppingCartID == cartId);
            return sc;
        }

        //An Alernate ?
        void DeleteItemFromCart(ShoppingCart cart)
        {
            _unitOfWork.ShoppingCart.Remove(cart);
            _unitOfWork.Save();
        }

        //An Alernate ?
        void UpdateSession(ShoppingCart cart)
        {
            var cartIconCount = _unitOfWork.ShoppingCart.GetAll(u => u.UserID == cart.UserID).ToList().Count();
            HttpContext.Session.SetInt32(SD.ShoppingCart, cartIconCount);
        }

        void Round_OffPriceValue(MenuItem menuItem)
        {
            menuItem.MenuItemPrice = Decimal.Round(menuItem.MenuItemPrice, 2);
        }
    }
}
