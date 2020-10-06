using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

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
