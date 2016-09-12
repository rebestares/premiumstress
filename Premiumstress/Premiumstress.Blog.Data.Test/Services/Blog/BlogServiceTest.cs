using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Premiumstress.Blog.Services.Blogs;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Data.Test.Services.Blog
{
    using Core.Domain.Blog;

    [TestClass]
    public class BlogServiceTest
    {
        private BlogService _blogService;
        private IRepository<Blog> _blogRepository;
        private IRepository<Keyword> _keywordRepository;
        private IUnitOfWork _unitOfWork;
        private DbContext _context;
        [TestInitialize]
        public void Initialize()
        {
            _context = new PremiumStressContext();
            _blogRepository = new Repository<Blog>(_context);
            _keywordRepository = new Repository<Keyword>(_context);
            _unitOfWork = new UnitOfWork();
            _blogService = new BlogService(_blogRepository, _keywordRepository, _unitOfWork);
        }

        [TestMethod]
        public void Query_All_Active_Blog_Posts()
        {
            var allBlogs = _blogService.GetAllBlogPosts(0, 5,"settings", "datePosted", "desc");
            foreach (var blog in allBlogs)
            {
                Trace.TraceInformation("Blog Title: {0}", blog.Title);
            }

            Trace.TraceInformation("Blog Count: {0}", allBlogs.Count);
        }


        [TestMethod]
        public void Query_Get_Blog_By_Tag()
        {

            var allBlogs = _blogService.GetAllBlogPostsByTag(0, 5, " tips");
            foreach (var blog in allBlogs)
            {
                Trace.TraceInformation("Blog Title: {0}", blog.Title);
            }

            Trace.TraceInformation("Blog Count: {0}", allBlogs.Count);
        }

        [TestMethod]
        public void Query_Get_Blog_By_UserId()
        {

            var allBlogs = _blogService.GetAllBlogPostsByUserId(0, 10, 1);
            foreach (var blog in allBlogs)
            {
                Trace.TraceInformation("Blog Title: {0}", blog.Title);
            }

            Trace.TraceInformation("Blog Count: {0}", allBlogs.Count);
        }


        [TestMethod]
        public void Query_Get_Blog_By_Category()
        {

            var allBlogs = _blogService.GetAllBlogPostsByCategory(0, 5, "news");
            foreach (var blog in allBlogs)
            {
                Trace.TraceInformation("Blog Title: {0}", blog.Title);
            }

            Trace.TraceInformation("Blog Count: {0}", allBlogs.Count);
        }

        [TestMethod]
        public void Query_Get_Popular_Blog_Posts()
        {
            var popularBlogPosts = _blogService.GetPopularBlogPosts();
            foreach (var blog in popularBlogPosts)
            {
                Trace.TraceInformation("Popular blog: {0}", blog.Title);
            }
        }

        [TestMethod]
        public void Query_Single_Blog()
        {
            var blog = _blogService.GetSingleBlogById(1,false);

            Trace.TraceInformation("Blog Title: {0}", blog.Title);
        }

        [TestMethod]
        public void Query_Single_Blog_Edit()
        {
            var blog = _blogService.GetSingleBlogById(1,true);
            Trace.TraceInformation("Blog Title: {0}", blog.Title);
            Trace.TraceInformation("View Count: {0}", blog.ViewCount);
        }

        [TestMethod]
        public void Query_Update_Blog()
        {
            var toBeUpdatedBlog = _blogService.GetSingleBlogById(1,false);
            toBeUpdatedBlog.Title = "Edited using test";
            var isSuccess = _blogService.UpdateBlog(toBeUpdatedBlog);
            Trace.TraceInformation("Is Updated?: {0}", isSuccess);
        }

        [TestMethod]
        public void Query_GetPromotedBlog_Blog()
        {
            var promotedBlog = _blogService.GetPromotedBlog();
            Trace.TraceInformation("Is Updated?: {0}", promotedBlog.Title);
        }

        [TestMethod]
        public void Query_Suggested_Blog()
        {
            var suggestedArticles = _blogService.GetSuggestedBlogs(4,1);
            foreach (var blog in suggestedArticles)
            {
                Trace.TraceInformation("Blog Title: {0}", blog.Title);
            }

            Assert.AreEqual(3,suggestedArticles.Count());
        }


        //[TestMethod]
        //public void Query_Insert_Blog()
        //{
        //    Blog blogObject = new Blog()
        //    {
        //        ID = 15,
        //        Title = "Valerie's Post",
        //        Content = "I love rebie",
        //        Keywords = new List<Keyword>()
        //        {
        //            new Keyword()
        //            {
        //                Keywords = "love love"
        //            },
        //            new Keyword()
        //            {
        //                Keywords = "heart heart"
        //            }
        //        }
        //    };

        //    var allBlogs = _blogService.GetAllBlogPosts(1, 15);
        //    Trace.TraceInformation("Blog count: {0}", allBlogs.Count);

        //    var isSuccess = _blogService.InsertBlog(blogObject);
        //    Trace.TraceInformation("Is successful?: {0}", isSuccess);

        //    allBlogs = _blogService.GetAllBlogPosts(1, 15);
        //    Trace.TraceInformation("Blog count: {0}", allBlogs.Count);
        //}

        [TestMethod]
        public void Query_Delete_Blog()
        {
            var blog = _blogService.GetSingleBlogById(13,false);
            var isSuccess = _blogService.DeleteBlog(blog);
            Trace.TraceInformation("Is blog deleted?: {0}", isSuccess);
        }

        [TestMethod]
        public void Query_Find_Blog_By_String()
        {
            var blogs = _blogService.FindBlogByWords("Fallout 4",1,5);

            foreach (var blog in blogs)
            {
                Trace.TraceInformation("Blog found: {0}", blog.Title);
            }
          
        }

        [TestMethod]
        public void Query_Get_Keywords()
        {
            var keywords = _blogService.GetKeywords().ToList();

            foreach (var model in keywords)
            {
                Trace.TraceInformation("{0}", model.Keywords);
            }

            Trace.TraceInformation("{0}", keywords.Count());


        }


    }
}
