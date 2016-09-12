using System.Collections.Generic;
using System.Linq;
using Premiumstress.Core.Domain.Blog;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Category
{
    using Core.Domain;

    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<BlogCategory> _blogCategoryRepository;

        #region ctor
        public CategoryService(
            IRepository<Category> categoryRepository,
            IRepository<BlogCategory> blogCategoryRepository)
        {
            _categoryRepository = categoryRepository;
            _blogCategoryRepository = blogCategoryRepository;
        }
        #endregion

        public List<Category> GetCategoriesFor(string type)
        {
            var query = _categoryRepository.Table;
            var categories = query.Where(a => a.Type == type).OrderBy(b => b.DisplayOrder).ToList();

            return categories;
        }

        public bool InsertCategory(Category category)
        {
            var query = _categoryRepository.Table;
            var lastRecord = query.OrderByDescending(a => a.ID).FirstOrDefault();

            category.ID = lastRecord?.ID + 1 ?? 1;

            var isSuccess = (_categoryRepository.Insert(category));
            return isSuccess;

        }

        public bool DeleteCategory(Category category)
        {
            var query = _categoryRepository.Table;
            var categoryToBeDeleted = query.FirstOrDefault(a => a.ID == category.ID);

            var isSuccess = (_categoryRepository.Delete(categoryToBeDeleted));
            return isSuccess;
        }

        public bool UpdateCategory(List<Category> categoryList)
        {
            var query = _categoryRepository.Table;
            var currentCategories = query.Select(a => a);
            foreach (var category in categoryList)
            {
                var categoryToBeUpdated = currentCategories.SingleOrDefault(a => a.ID == category.ID);

                if (categoryToBeUpdated != null)
                {
                    categoryToBeUpdated.Name = category.Name;

                    if (!_categoryRepository.Update(categoryToBeUpdated)) return false;
                }
            }
            return true;
        }

        public bool UpdateBlogCategory(BlogCategory category, int blogId)
        {
            var query = _blogCategoryRepository.Table;

            if (category == null) return false;

            var currentCategory = query.SingleOrDefault(a => a.BlogID == blogId);
            if (currentCategory?.Blog != null)
            {
                currentCategory.BlogID = blogId;
                currentCategory.CategoryID = category.CategoryID;
                _blogCategoryRepository.Update(currentCategory);
            }
            else
            {
                var newBlogCategory = new BlogCategory
                {
                    BlogID = blogId,
                    CategoryID = category.CategoryID
                };

                _blogCategoryRepository.Update(newBlogCategory);
            }

            return true;
        }
    }
}
