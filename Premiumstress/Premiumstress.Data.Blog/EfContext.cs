using System;
using System.Linq;
using System.Linq.Expressions;
using Premiumstress.Core;

namespace Premiumstress.Data.Blog
{
    public class EfContext<T> : IRepository<T> where T : BaseEntity
    {
        public IQueryable<T> Table { get; private set; }
        public IQueryable<T> TableUntracked { get; private set; }

        public T Create()
        {
            throw new NotImplementedException();
        }

        public T GetById(object id)
        {
            throw new NotImplementedException();
        }

        public T Attach(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Insert(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Update(T entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Expand(IQueryable<T> query, string path)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Expand<TProperty>(IQueryable<T> query, Expression<Func<T, TProperty>> path)
        {
            throw new NotImplementedException();
        }

        public bool IsModified(T entity)
        {
            throw new NotImplementedException();
        }

        public bool? AutoCommitEnabled { get; set; }

        public int SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}