using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stripe;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;
using TakeAwayExample2.Models.ViewModels;
using TakeAwayExample2.Utility;

namespace TakeAwayExample2.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public SummaryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //Need to bind property because we not passing the detailCart variable to the OnPost function
        [BindProperty]
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
            detailCart.OrderHeader.PhoneNumer = user.PhoneNumber;

            return Page();
        }

        public IActionResult OnPost(string stripeToken)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            /**
             * Here we are assiging values to the ShoppingCartOrdersViewModel
             * The order headers is a single object that holds info for the client.
             * The ShoppingCartList is a list of all the items that the client wants to buy 
             * **/

            detailCart.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(sc => sc.UserID == claim.Value).ToList();

            detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailCart.OrderHeader.OrderDate = DateTime.Now;
            detailCart.OrderHeader.UserID = claim.Value;
            detailCart.OrderHeader.Status = SD.PaymentStatusPending;
            detailCart.OrderHeader.CollectionTime = Convert.ToDateTime(detailCart.OrderHeader.CollectionDate.ToShortDateString() + " " + detailCart.OrderHeader.CollectionTime.ToShortTimeString());

            _unitOfWork.OrderHeader.Add(detailCart.OrderHeader);
            _unitOfWork.Save();

            /**
             * Create a list of order detials that will hold the info of each item that client wants to buy
             * **/
            List<OrderDetail> orderDetails = new List<OrderDetail>();

            foreach (var item in detailCart.ShoppingCartList)
            {
                item.MenuItem = _unitOfWork.MenuItem.GetFirstOrDefault(mi => mi.MenuItemID == item.MenuItemID);

                OrderDetail orderDetail = new OrderDetail
                {
                    MenuItemID = item.MenuItemID,
                    OrderHeaderID = detailCart.OrderHeader.OrderHeaderID,
                    OrderDescription = item.MenuItem.MenuItemDescription,
                    OrderName = item.MenuItem.MenuItemName,
                    Price = item.MenuItem.MenuItemPrice,
                    OrderCount = item.ItemCount
                };

                //Sum the totals of each item quantity (ie the price of the item * number of those same items)
                detailCart.OrderHeader.OrderTotal += (orderDetail.OrderCount * orderDetail.Price);
                //Add each item of type that user selected to the details to the database
                _unitOfWork.OrderDetail.Add(orderDetail);
            }

            _unitOfWork.ShoppingCart.RemoveRange(detailCart.ShoppingCartList);

            //Update the session (The count of the bakset item)
            HttpContext.Session.SetInt32(SD.ShoppingCart, 0);

            _unitOfWork.Save();

            if (stripeToken != null)
            {
                var options = new ChargeCreateOptions
                {
                    //Convert the amount into cents
                    Amount = Convert.ToInt32(detailCart.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID: " + detailCart.OrderHeader.OrderHeaderID,
                    Source = stripeToken
                };

                /**
                 * This determines the status of the payment using the stripe api
                 * **/
                var service = new ChargeService();
                Charge charge = service.Create(options);

                detailCart.OrderHeader.TransactionID = charge.Id;

                if (charge.Status.ToLower() == "succeeded")
                {
                    detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    detailCart.OrderHeader.Status = SD.StatusSubmitted;
                }
                else
                {
                    detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
            }
            else
            {
                detailCart.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
            }

            _unitOfWork.Save();

            return RedirectToPage("/Customer/Cart/OrderConfirmation", new { id = detailCart.OrderHeader.OrderHeaderID });
        }
    }
}
