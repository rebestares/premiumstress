using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Premiumstress.Blog.Services.Tag
{
    using Core.Domain;

    public interface ITagService
    {
        List<Keyword> GetAllKeywords();
        Keyword GetSpecificKeyword(Keyword keyword);
        bool AddNewTag(Keyword keyword);
        bool UpdateTag(Keyword keyword);
        bool DeleteTag(Keyword keyword);
    }
}
