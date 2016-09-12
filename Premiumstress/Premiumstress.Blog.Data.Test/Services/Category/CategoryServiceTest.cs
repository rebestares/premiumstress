using System.Data.Entity;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Core.Domain.Blog;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Data.Test.Services.Category
{
    using Core.Domain;

    [TestClass]
    public class CategoryServiceTest
    {
        private CategoryService _categoryService;
        private IRepository<Category> _categoryRepository;
        private IRepository<BlogCategory> _blogCategoryRepository;
        private DbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new PremiumStressContext();
            _categoryRepository = new Repository<Category>(_context);
            _blogCategoryRepository = new Repository<BlogCategory>(_context);
            _categoryService = new CategoryService(_categoryRepository, _blogCategoryRepository);
        }

        [TestMethod]
        public void Query_Category()
        {
            var categories = _categoryService.GetCategoriesFor("blog");

            foreach (var category in categories)
            {
                Trace.TraceInformation("Category: {0}", category.Name);
            }

        }

        [TestMethod]
        public void Query_Update_Blog_Category()
        {

            var category = new BlogCategory()
            {
                BlogID = 15,
                CategoryID = 3,
            };

            var isAdded = _categoryService.UpdateBlogCategory(category,15);

            Trace.TraceInformation("Is successfully updated: {0}", isAdded);
        }
    }
}
