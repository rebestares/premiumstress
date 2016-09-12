using System.Collections.Generic;
using Premiumstress.Core.Domain.Blog;

namespace Premiumstress.Blog.Services.Category
{
    using Core.Domain;

    public interface ICategoryService
    {
        List<Category> GetCategoriesFor(string type);
        bool InsertCategory(Category category);
        bool DeleteCategory(Category category);
        bool UpdateCategory(List<Category> categoryList);
        bool UpdateBlogCategory(BlogCategory category, int blogId);
    }
}
