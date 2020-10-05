﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TakeAwayExample2.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T GetSingle(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includedProperties = null
            );

        void AddItem(T entity);

        void Remove(int id);

        void DeleteItem(T entity);
    }
}
