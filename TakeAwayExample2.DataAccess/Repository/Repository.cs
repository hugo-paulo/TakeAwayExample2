using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TakeAwayExample2.DataAccess.Repository.IRepository;

namespace TakeAwayExample2.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        //This is a internal class
        internal DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            this._dbSet = context.Set<T>();
        }

        public void CreateItem(T entity)
        {
            _dbSet.Add(entity);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //Remember the comma has not space after it, thus when calling this method we dont use space after comma
                //Alternate
                //foreach (var includeProperty in includeProperties.Split(new char[] { ', ' }, StringSplitOptions.RemoveEmptyEntries))
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                //Remember the comma has not space after it, thus when calling this method we dont use space after comma
                //Alternate
                //foreach (var includeProperty in includeProperties.Split(new char[] { ', ' }, StringSplitOptions.RemoveEmptyEntries))
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.FirstOrDefault();
        }

        public T GetSingle(int id)
        {
            return _dbSet.Find(id);
        }

        public void Remove(int id)
        {
            T entityToRemove = _dbSet.Find(id);
            //Good use of function overloading (convert the argument and call the generic function)
            DeleteItem(entityToRemove);
        }

        public void DeleteItem(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}
