using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Premiumstress.Core.Domain;

namespace Premiumstress.Data.Blog
{
    using Core.Domain.Blog;
    public interface IUnitOfWork :IDisposable
    {
        DbSet<T> Set<T>() where T : class;
        int Commit();
    }
}
