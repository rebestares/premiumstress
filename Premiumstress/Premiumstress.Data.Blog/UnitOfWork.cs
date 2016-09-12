using System;
using System.Data.Entity;
using Premiumstress.Core.Domain;

namespace Premiumstress.Data.Blog
{
    using Core.Domain.Blog;

    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DbContext _context;

         
        public UnitOfWork()
        {
            _context = new PremiumStressContext();
        }

        public UnitOfWork(DbContext context)
        {
            _context = context;
        }

        public DbSet<T> Set<T>() where T : class
        {
            return _context.Set<T>();
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        internal DbContext Context
        {
            get { return _context; }
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
