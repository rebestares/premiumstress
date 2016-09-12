using System.Data.Entity;

namespace Premiumstress.Data.Blog
{
    public interface IDbContext
    {
        int SaveChanges();
    }
}