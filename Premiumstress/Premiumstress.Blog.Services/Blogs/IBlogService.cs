using System.Collections.Generic;
using Premiumstress.Core;
using Premiumstress.Core.Domain;

namespace Premiumstress.Blog.Services.Blogs
{
    using Core.Domain.Blog;

    public interface IBlogService
    {
        IPagedList<Blog> GetAllBlogPosts(int pageIndex, int pageSize, string module, string sortProperty, string sortOrder);
        IPagedList<Blog> GetAllBlogPostsByTag(int pageIndex, int pageSize,
            string tag);

        IPagedList<Blog> GetAllBlogPostsByCategory(int pageIndex, int pageSize, string category);
        IPagedList<Blog> FindBlogByWords(string word, int pageIndex, int pageSize);
        IEnumerable<Blog> GetSuggestedBlogs(int numOfBlogs, int? blogId);
        IPagedList<Blog> GetAllBlogPostsByUserId(int pageIndex, int pageSize,
            int userId);
        bool UpdateKeywords(List<Keyword> keywords, Blog blog);
        Blog GetPromotedBlog();
        bool PromoteBlog(int id);
        bool ApproveBlog(int id, bool approvalStatus);
        List<Blog> GetPopularBlogPosts();
        Blog GetSingleBlogById(int id, bool isEdit);
        bool UpdateBlog(Blog blog);
        Blog InsertBlog(Blog blog);
        bool DeleteBlog(Blog blog);
        List<string> GetKeywordsOfBlog(int id);
        IEnumerable<Keyword> GetKeywords();

    }
}