using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _ctx;

        public MenuItemRepository(ApplicationDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public void UpdateItem(MenuItem menuItem)
        {
            var obj = _ctx.MenuItem.FirstOrDefault(m => m.MenuItemID == menuItem.MenuItemID);

            obj.MenuItemName = menuItem.MenuItemName;
            obj.CategoryID = menuItem.CategoryID;
            obj.MenuItemDescription = menuItem.MenuItemDescription;
            obj.FoodTypeID = menuItem.FoodTypeID;
            obj.MenuItemPrice = menuItem.MenuItemPrice;

            if (menuItem.MenuItemImage != null)
            {
                obj.MenuItemImage = menuItem.MenuItemImage;
            }

            _ctx.SaveChanges();
        }
    }
}
