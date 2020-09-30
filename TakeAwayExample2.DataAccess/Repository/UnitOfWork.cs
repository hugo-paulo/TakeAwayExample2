using System;
using System.Collections.Generic;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _ctx;

        public UnitOfWork(ApplicationDbContext ctx)
        {
            _ctx = ctx;
            Category = new CategoryRepository(_ctx);
        }

        public ICategoryRepository Category { get; private set; }

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
