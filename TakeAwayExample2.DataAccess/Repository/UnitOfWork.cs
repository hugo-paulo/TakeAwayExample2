using System;
using System.Collections.Generic;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _ctx;
        private readonly LoginDbContext _loginCtx;

        public UnitOfWork(ApplicationDbContext ctx, LoginDbContext loginCtx)
        {
            _ctx = ctx;
            _loginCtx = loginCtx;
            Category = new CategoryRepository(_ctx);
            FoodType = new FoodTypeRepository(_ctx);
            MenuItem = new MenuItemRepository(_ctx);
            User = new UserRepository(_loginCtx);
        }

        public ICategoryRepository Category { get; private set; }
        public IFoodTypeRepository FoodType { get; private set; }
        public IMenuItemRepository MenuItem { get; private set; }
        public IUserRepository User { get; private set; }

        public void Dispose()
        {
            _ctx.Dispose();
        }

        public void Save()
        {
            _ctx.SaveChanges();
        }
    }
}
