using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using Premiumstress.Core;
using Premiumstress.Core.Extensions;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Blogs
{

    using Core.Domain.Blog;
    using Core.Domain;

    public class BlogService : IBlogService
    {
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<Keyword> _keywordRepository;
        private IUnitOfWork _unitOfWork;

        #region Ctor
        public BlogService
            (
            IRepository<Blog> blogRepository,
            IRepository<Keyword> keywordRepository,
            IUnitOfWork unitOfWork
            )
        {
            _blogRepository = blogRepository;
            _keywordRepository = keywordRepository;
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets popular blog posts
        /// </summary>
        /// <returns>Blogs</returns>
        public List<Blog> GetPopularBlogPosts()
        {
            var query = _blogRepository.Table;
            var blogs = query.Where(a => a.IsDeleted != true && a.IsApproved.Value).ToList();
            var featuredBlogs = blogs.Count() < 5 ? blogs : blogs.OrderByDescending(a => a.ViewCount).Take(5).ToList();

            return featuredBlogs;
        }

        /// <summary>
        /// Gets a single blog using ID
        /// </summary>
        /// <param name="id">BlogID</param>
        /// <param name="isEdit">Is blog for edit?</param>
        /// <returns>Blog</returns>
        public Blog GetSingleBlogById(int id, bool isEdit)
        {
            Expression<Func<Blog, bool>> query;
            if (isEdit)
                query = blog => blog.ID == id
                                     &&
                                     blog.IsDeleted.Value != true;
            else
            {
                query = blog => blog.ID == id
                                     &&
                                     blog.IsDeleted.Value != true
                                     &&
                                     blog.IsApproved.Value;
            }

            var singleBlog = _blogRepository.Table.SingleOrDefault(query);

            if (singleBlog != null && !isEdit)
            {
                singleBlog.ViewCount = singleBlog.ViewCount != null ? singleBlog.ViewCount + 1 : 1;
                _blogRepository.Update(singleBlog);
            }

            return singleBlog;
        }

        /// <summary>
        /// Get all blogs who are not deleted
        /// </summary>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="pageSize">The number of pages to be returned</param>
        /// <param name="module">The module calling this service</param>
        ///  <param name="sortProperty">The property that will be used to sort the blogs</param>
        ///  <param name="sortOrder">To determine if it's ascending or descending order</param>
        /// <returns>Blogs</returns>
        public IPagedList<Blog> GetAllBlogPosts(int pageIndex, int pageSize, string module, string sortProperty, string sortOrder)
        {

            var blogs = _blogRepository.Table;

            if (module == "settings")
            {
                blogs = (from blog in blogs
                         where blog.IsDeleted != true
                         select blog)
                        .OrderBy($"{sortProperty} {sortOrder}");
            }
            else if (module == "blog")
            {
                blogs = from blog in blogs
                        where blog.IsDeleted != true
                              &&
                              blog.IsApproved.Value
                        orderby blog.ID descending
                        select blog;
            }

            var blogPosts = new PagedList<Blog>(blogs, pageIndex, pageSize);
            return blogPosts;

        }

        /// <summary>
        /// Get blogs using tag keywords
        /// </summary>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="pageSize">The number of pages to be returned</param>
        /// <param name="tag">Find blog using tags</param>
        /// <returns>Blogs</returns>
        public IPagedList<Blog> GetAllBlogPostsByTag(int pageIndex, int pageSize,
            string tag)
        {
            var blogs = _blogRepository.Table;

            if (tag != null)
            {
                tag = tag.Replace('-', ' ').ToLower();

                blogs = from blog in blogs
                        where blog.Keywords.Any(keyword => keyword.Keywords.ToLower().Trim() == tag)
                              &&
                              blog.IsDeleted != true
                              &&
                              blog.IsApproved.Value
                        orderby blog.ID descending
                        select blog;
            }

            var blogPosts = new PagedList<Blog>(blogs, pageIndex, pageSize);
            return blogPosts;
        }

        /// <summary>
        /// Get blogs using categories
        /// </summary>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="pageSize">The number of pages to be returned</param>
        /// <param name="category">Find blog using category</param>
        /// <returns>Blogs</returns>
        public IPagedList<Blog> GetAllBlogPostsByCategory(int pageIndex, int pageSize,
           string category)
        {
            var blogs = _blogRepository.Table;

            if (category != null)
            {
                category = category.Replace('-', ' ');

                blogs = from blog in blogs
                        where blog.BlogCategories.Any(ctgry => ctgry.Category.Name.ToLower() == category)
                              &&
                              blog.IsDeleted != true
                              &&
                              blog.IsApproved.Value
                        orderby blog.ID descending
                        select blog;
            }

            var blogPosts = new PagedList<Blog>(blogs, pageIndex, pageSize);
            return blogPosts;
        }

        /// <summary>
        /// Get blogs using categories
        /// </summary>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="pageSize">The number of pages to be returned</param>
        /// <param name="userId">Find blog using userId</param>
        /// <returns>Blogs</returns>
        public IPagedList<Blog> GetAllBlogPostsByUserId(int pageIndex, int pageSize,
          int userId)
        {
            var blogs = from blog in _blogRepository.Table
                        where blog.User.ID == userId
                              &&
                              blog.IsDeleted != true
                              &&
                              blog.IsApproved.Value
                        orderby blog.ID descending
                        select blog;

            var blogPosts = new PagedList<Blog>(blogs, pageIndex, pageSize);
            return blogPosts;
        }


        /// <summary>
        /// Get blogs using keywords
        /// </summary>
        /// <param name="word">Find blog using words, usually used on search functionality</param>
        /// <param name="pageIndex">Current page number</param>
        /// <param name="pageSize">The number of pages to be returned</param>
        /// <returns>Blogs</returns>
        public IPagedList<Blog> FindBlogByWords(string word, int pageIndex, int pageSize)
        {
            var query = _blogRepository.Table;
            string[] words = word.Split(new[] { ',', ' ' });

            foreach (string searchWord in words)
            {
                query = from blog in _blogRepository.Table
                        where blog.Title.Contains(searchWord)
                              &&
                              blog.IsDeleted != true
                              &&
                              blog.IsApproved.Value
                        orderby blog.ID descending
                        select blog;
            }

            var blogPosts = new PagedList<Blog>(query, pageIndex, pageSize);
            return blogPosts;

        }

        /// <summary>
        /// Update blog from mapping collection
        /// </summary>
        /// <param name="blog">>Blog object to be updated</param>
        /// <returns>Bollean if successfully updated or not</returns>
        public bool UpdateBlog(Blog blog)
        {
            var isSuccess = (_blogRepository.Update(blog));
            return isSuccess;
        }

        /// <summary>
        /// Insert new blog from mapping collection
        /// </summary>
        /// <param name="blog">>Blog object to be inserted</param>
        /// <returns>Bollean if successfully added or not</returns>
        public Blog InsertBlog(Blog blog)
        {
            var query = _blogRepository.Table;

            var lastRecord = query.OrderByDescending(u => u.ID).FirstOrDefault();

            blog.ID = lastRecord?.ID + 1 ?? 1;
            blog.DatePosted = DateTime.Today;
            var firstOrDefault = blog.BlogCategories.FirstOrDefault();
            if (firstOrDefault != null) firstOrDefault.BlogID = blog.ID;
            _blogRepository.Insert(blog);
            return blog;
        }

        /// <summary>
        /// Delete and Update blog from mapping collection
        /// </summary>
        /// <param name="blog">>Blog object to be deleted</param>
        /// <returns>Bollean if successfully deleter or not</returns>
        public bool DeleteBlog(Blog blog)
        {
            if (blog != null)
            {
                blog.IsDeleted = true;
                blog.DateDeleted = DateTime.Today;
                blog.DateUpdated = DateTime.Today;
            }

            var isSuccess = (_blogRepository.Update(blog));
            return isSuccess;
        }

        /// <summary>
        /// Gets keywords of blog from mapping collection
        /// </summary>
        /// <param name="id">>The blog ID to get keywords</param>
        /// <returns>List of keywords from the requested blog</returns>
        public List<string> GetKeywordsOfBlog(int id)
        {
            var keywordsList = (from keyword in _keywordRepository.Table
                                where keyword.Blogs.Any(blog => blog.ID == id)
                                select keyword);

            var keywords = new List<string>();

            foreach (var keyword in keywordsList)
            {
                keywords.Add(keyword.Keywords);
            }

            return keywords;
        }

        /// <summary>
        /// Gets suggested blogs from mapping collection
        /// </summary>
        /// <param name="numOfBlogs">Number of blogs that you want in return</param>
        /// <param name="blogId">The blogId that you wish to remove from the returned list
        /// This is used to prevent the promoted blog to be returned as suggested blog
        /// </param>
        /// <returns>Suggested blogs mapping collection</returns>
        public IEnumerable<Blog> GetSuggestedBlogs(int numOfBlogs, int? blogId)
        {
            var blogRepo = _blogRepository.Table;

            List<int> allBlogIds = (from blog in blogRepo
                                    where blog.IsDeleted != true && blog.IsApproved.Value
                                    select blog.ID).ToList();

            if (blogId != null)
                allBlogIds.Remove((int)blogId);

            List<Blog> suggestedBlogs = new List<Blog>();

            for (var i = 0; i < numOfBlogs; i++)
            {
                var randomId = allBlogIds.RandomElement();
                var blog = blogRepo.SingleOrDefault(a => a.ID == randomId);
                suggestedBlogs.Add(blog);
                allBlogIds.Remove(randomId);
            }

            return suggestedBlogs;
        }

        /// <summary>
        /// Gets promoted blogs from mapping collection
        /// </summary>
        /// <returns>Blog</returns>
        public Blog GetPromotedBlog()
        {
            var promotedBlog = (from blog in _blogRepository.Table
                                where blog.IsPromoted.Value
                                       &&
                                       blog.IsDeleted.Value != true
                                       &&
                                       blog.IsApproved.Value
                                select blog).FirstOrDefault();

            return promotedBlog;
        }

        /// <summary>
        /// Updates the promoted blog 
        /// </summary>
        /// <param name="id">>The blog ID to be promoted</param>
        /// <returns>Boolean if success or not</returns>
        public bool PromoteBlog(int id)
        {
            var query = _blogRepository.Table;

            var oldPromotedBlog = GetPromotedBlog();
            if (oldPromotedBlog != null)
                oldPromotedBlog.IsPromoted = null;

            UpdateBlog(oldPromotedBlog);

            var newPromotedBlog = query.SingleOrDefault(a => a.ID == id);
            if (newPromotedBlog != null)
                newPromotedBlog.IsPromoted = true;

            return UpdateBlog(newPromotedBlog);
        }

        /// <summary>
        /// Approve or disapprove the blog
        /// </summary>
        /// <param name="id">>The blog ID to be approved/disapproved</param>
        /// <param name="approvalStatus">>Status of the blog</param>
        /// <returns>Boolean if success or not</returns>
        public bool ApproveBlog(int id, bool approvalStatus)
        {
            var query = _blogRepository.Table;
            var blog = query.SingleOrDefault(a => a.ID == id);
            if (blog == null) return false;
            blog.IsApproved = approvalStatus;
            _blogRepository.Update(blog);
            return true;
        }


        /// <summary>
        /// Update keywords
        /// </summary>
        /// /// <param name="keywords">>The list of keywords to be updated</param>
        /// <param name="blog">>The blog object to be updated</param>
        /// <returns>Boolean if success or not</returns>
        public bool UpdateKeywords(List<Keyword> keywords, Blog blog)
        {
            var query = _keywordRepository.Table;
            if (blog == null) return false;

            var currBlog = _blogRepository.Table.FirstOrDefault(a => a.ID == blog.ID);

            if (currBlog != null)
            {
                currBlog.Keywords?.Clear();

                foreach (var keyword in keywords)
                {
                    var keywordObj = query.FirstOrDefault(a => a.Keywords.ToString() == keyword.Keywords.ToLower());
                    if (keywordObj != null)
                    {
                        currBlog.Keywords?.Add(keywordObj);
                    }
                    else
                    {
                        var newKeyword = new Keyword()
                        {
                            Keywords = keyword.Keywords.ToLower()
                        };

                        currBlog.Keywords?.Add(newKeyword);
                    }
                }

                _blogRepository.Update(currBlog);
            }
            return true;
        }

        /// <summary>
        /// Get all keywords
        /// </summary>
        /// <returns>Keywords</returns>
        public IEnumerable<Keyword> GetKeywords()
        {
            var keywordList = new List<Keyword>();

            var keywordsCollection = from blog in _blogRepository.Table
                                     where blog.IsApproved.Value && blog.IsDeleted != true
                                     select blog.Keywords.Select(a => a);

            foreach (var keywords in keywordsCollection)
            {
                keywordList.AddRange(keywords);
            }

            return keywordList.Distinct();
        }

        #endregion
    }
}