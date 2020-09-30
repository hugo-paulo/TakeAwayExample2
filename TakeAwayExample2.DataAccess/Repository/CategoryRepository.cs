using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _ctx;

        public CategoryRepository(ApplicationDbContext ctx) : base(ctx) 
        {
            _ctx = ctx;
        }

        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return _ctx.Category.Select(c => new SelectListItem() 
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            });
        }

        public void Update(Category category)
        {
            var obj = _ctx.Category.FirstOrDefault(c => c.CategoryID == category.CategoryID);

            obj.CategoryName = category.CategoryName;
            obj.DisplayOrder = category.DisplayOrder;

            _ctx.SaveChanges();
        }
    }
}
