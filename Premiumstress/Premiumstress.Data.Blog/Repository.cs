using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;

namespace Premiumstress.Data.Blog
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DbContext _context;
        private IDbSet<T> _entities;

        public Repository(DbContext context)
        {
            this._context = context;
        }

        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities as DbSet<T>;
            }
        }


        public virtual IQueryable<T> Table
        {
            get { return Entities; }
        }

        public IQueryable<T> TableUntracked { get; set; }
        public bool? AutoCommitEnabled { get; set; }

        public T Create()
        {
            return Entities.Create();
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public T Attach(T entity)
        {
            return Entities.Attach(entity);
        }

        public bool Insert(T entity)
        {
            bool isSuccess;

            if (entity == null)
                throw new ArgumentNullException("entity");

            try
            {
                Entities.Add(entity);

                _context.SaveChanges();
                isSuccess = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                isSuccess = false;
            }
            return isSuccess;

        }

        public bool Update(T entity)
        {
            if (entity == null) return false;

            try
            {
                this.Entities.AddOrUpdate(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                // ignored
            }

            return true;
        }

        public bool Delete(T entity)
        {
            bool isSuccess;
            if (entity == null)
                throw new ArgumentNullException("entity");
            try
            {
                Entities.Remove(entity);
                _context.SaveChanges();
                isSuccess = true;
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
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
    }
}