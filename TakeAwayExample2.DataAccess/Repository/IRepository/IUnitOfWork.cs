using System;
using System.Collections.Generic;
using System.Text;

namespace TakeAwayExample2.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IFoodTypeRepository FoodType { get; }

        void Save();
    }
}
