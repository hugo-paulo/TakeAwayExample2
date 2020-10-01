using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class FoodTypeRepository : Repository<FoodType>, IFoodTypeRepository
    {
        private readonly ApplicationDbContext _ctx;

        public FoodTypeRepository(ApplicationDbContext ctx) : base(ctx)
        {
            _ctx = ctx;
        }

        public IEnumerable<SelectListItem> GetFoodTypeListForDropDown()
        {
            return _ctx.FoodType.Select(c => new SelectListItem()
            {
                Text = c.FoodTypeName,
                Value = c.FoodTypeID.ToString()
            });
        }

        public void UpdateItem(FoodType foodType)
        {
            var obj = _ctx.FoodType.FirstOrDefault(f => f.FoodTypeID == foodType.FoodTypeID);

            obj.FoodTypeName = foodType.FoodTypeName;

            _ctx.SaveChanges();
        }
    }
}
