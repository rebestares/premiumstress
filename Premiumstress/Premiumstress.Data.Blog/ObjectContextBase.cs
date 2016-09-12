using System.Data.Entity;

namespace Premiumstress.Data.Blog
{
    public abstract class ObjectContextBase : DbContext, IDbContext
    {
        protected ObjectContextBase(string nameOrConnectionString) : base(nameOrConnectionString)
        {
        }

        protected ObjectContextBase()
        {
        }

        //public new DbSet<TEntity> Set<TEntity>() where TEntity : class

        //{
        //    return base.Set<TEntity>();
        //}
    }
}