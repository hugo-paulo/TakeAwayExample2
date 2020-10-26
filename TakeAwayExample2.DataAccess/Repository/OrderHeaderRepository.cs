using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _ctx;

        public OrderHeaderRepository(ApplicationDbContext ctx) : base(ctx)
        {
            ctx = _ctx;
        }

        public void UpdateItem(OrderHeader orderHeader)
        {
            var obj = _ctx.OrderHeader.FirstOrDefault(o => o.OrderHeaderID == orderHeader.OrderHeaderID);

            _ctx.OrderHeader.Update(obj);

            _ctx.SaveChanges();
        }
    }
}
