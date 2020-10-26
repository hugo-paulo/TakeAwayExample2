using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _ctx;

        public OrderDetailRepository(ApplicationDbContext ctx) : base(ctx)
        {
            ctx = _ctx;
        }

        public void UpdateItem(OrderDetail orderDetail)
        {
            var obj = _ctx.OrderDetail.FirstOrDefault(o => o.OrderDetailID == orderDetail.OrderDetailID);

            _ctx.OrderDetail.Update(obj);

            _ctx.SaveChanges();
        }
    }
}
