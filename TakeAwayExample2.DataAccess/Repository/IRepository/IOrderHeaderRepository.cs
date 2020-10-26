using System;
using System.Collections.Generic;
using System.Text;
using TakeAwayExample2.Models;

namespace TakeAwayExample2.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void UpdateItem(OrderHeader orderHeader);
    }
}
