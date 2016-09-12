using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI;
using AutoMapper;
using Premiumstress.Blog.Services.Blogs;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Blog.Services.Image;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Services.Video;
using Premiumstress.Blog.Website.Models;
using Premiumstress.Core;

namespace Premiumstress.Blog.Website.Controllers
{
    using Core.Domain;
    using Core.Domain.Blog;

    public class BlogController : Controller
    {
        //
        // GET: /Blog/

        #region Fields

        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly IVideoService _videoService;

        #endregion

        #region ctor


        public BlogController(
            IBlogService blogService,
            ICategoryService categoryService,
            IImageService imageService,
            IUserService userService,
            IVideoService videoService)
        {
            _blogService = blogService;
            _categoryService = categoryService;
            _imageService = imageService;
            _userService = userService;
            _videoService = videoService;
        }

        #endregion

        //public ActionResult Search(string word, int? page)
        //{
        //    //TempData["isLogged"] = Globals.Methods.IsUserLoggedIn();

        //    //word = Globals.Methods.ConvertString(word, false);

        //    var pageDetails = new
        //    {
        //        Page = page ?? 1,
        //        Word = word
        //    };

        //    return View("Search", null, pageDetails);
        //}

        //public ActionResult Page(int? page, int? category)
        //{
        //    //TempData["isLogged"] = Globals.Methods.IsUserLoggedIn();
        //    var pageDetails = new
        //    {
        //        Page = page,
        //        category = category
        //    };
        //    return View(pageDetails);
        //}

        public ActionResult Index()
        {
            var allBlogs = _blogService.GetAllBlogPosts(0, 5, "blog", "ID", "desc");
            return View();
        }

        [Authorize]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize]
        public ActionResult Edit(string title, int? id)
        {
            var blogId = id ?? 0;
            var singleBlog = _blogService.GetSingleBlogById(blogId, true);
            if (singleBlog == null)
                return View("Add", null);
            ViewBag.Title = singleBlog.Title;

            return View("Edit", null, blogId);
        }

        public ActionResult NotFound()
        {
            return View();
        }

        //public ActionResult Tag(string tag, int? page)
        //{
        //    //TempData["isLogged"] = Globals.Methods.IsUserLoggedIn();
        //    var pageDetails = new
        //    {
        //        Page = page ?? 1,
        //        Tag = tag
        //    };

        //    //if (tag != null)
        //    // ViewBag.Title = Globals.Methods.ConvertString(tag, true) + " Archives";

        //    return View("Tag", null, pageDetails);
        //}

        //public ActionResult Category(string category, int? page)
        //{
        //    //TempData["isLogged"] = Globals.Methods.IsUserLoggedIn();
        //    var pageDetails = new
        //    {
        //        Page = page ?? 1,
        //        Category = category
        //    };

        //    //if (tag != null)
        //    // ViewBag.Title = Globals.Methods.ConvertString(tag, true) + " Archives";

        //    return View("Category", null, pageDetails);
        //}



        #region Methods

        public ActionResult Article(string blogTitle, int? id)
        {
            var blogId = id ?? 0;
            var singleBlog = _blogService.GetSingleBlogById(blogId, true);

            if (singleBlog == null)
                return View("Index", null);

            ViewBag.Title = singleBlog.Title;

            var model = Mapper.Map<Blog, BlogModel>(singleBlog);

            return View("Article", null, model);
        }

        public ActionResult GetSingleBlog(int? id, bool isEdit)
        {
            var blogId = id ?? 0;
            var blog = _blogService.GetSingleBlogById(blogId, isEdit);
            var model = Mapper.Map<Blog, BlogModel>(blog);

            if (model != null)
                model.IsOwner = IsOwner(model.User.ID);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFeatured()
        {
            var featuredBlogs = _blogService.GetPopularBlogPosts();
            var featuredBlogList = featuredBlogs.Select(Mapper.Map<Blog, BlogModel>).ToList();

            return Json(featuredBlogList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBlogs(int? pageNumber,
                                     string module,
                                     string sortProperty,
                                     string sortOrder)
        {
            var indexPage = pageNumber - 1 ?? 0;

            sortProperty = sortProperty ?? "ID";
            sortOrder = sortOrder ?? "desc";

            var blogs = _blogService.GetAllBlogPosts(indexPage, 10, module, sortProperty, sortOrder);
            var blogList = new List<BlogModel>();

            foreach (var blog in blogs)
            {
                var model = Mapper.Map<Blog, BlogModel>(blog);

                if (model.User != null)
                    model.IsOwner = IsOwner(model.User.ID);

                model.SanitizeContent();
                blogList.Add(model);
            }

            var blogReturnObj = new
            {
                BlogList = blogList,
                CurrentPage = pageNumber,
                TotalBlogCount = blogs.TotalCount,
                WithCategory = false,
                WithTag = false,
                Module = module,
                SortOrder = sortOrder,
                SortProperty = sortProperty
            };

            return Json(blogReturnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult FindBlogs(string word, int? page, string module)
        {
            var indexPage = page - 1 ?? 0;
            var foundBlogs = _blogService.FindBlogByWords(word, indexPage, 10);

            var blogList = new List<BlogModel>();

            foreach (var blog in foundBlogs)
            {
                var model = Mapper.Map<Blog, BlogModel>(blog);
                model.IsOwner = IsOwner(model.User.ID);
                model.SanitizeContent();
                blogList.Add(model);
            }

            var blogReturnObj = new
            {
                BlogList = blogList,
                CurrentPage = page,
                TotalBlogCount = foundBlogs.TotalCount,
                WithCategory = false,
                WithTag = false,
                Module = module
            };

            return Json(blogReturnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBlogsByTag(int? pageNumber, string tag)
        {
            var indexPage = pageNumber - 1 ?? 0;
            var blogs = _blogService.GetAllBlogPostsByTag(indexPage, 10, tag);
            var blogList = new List<BlogModel>();

            foreach (var blog in blogs)
            {
                var model = Mapper.Map<Blog, BlogModel>(blog);
                model.SanitizeContent();
                blogList.Add(model);
            }

            var blogReturnObj = new
            {
                BlogList = blogList,
                CurrentPage = pageNumber,
                TotalBlogCount = blogs.TotalCount,
                WithCategory = false,
                WithTag = new
                {
                    WithTag = true,
                    TagName = tag.Replace('-', ' ')
                }
            };

            return Json(blogReturnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBlogsByCategory(int? pageNumber, string categoryName)
        {
            var indexPage = pageNumber - 1 ?? 0;
            var blogs = _blogService.GetAllBlogPostsByCategory(indexPage, 10, categoryName);
            var blogList = new List<BlogModel>();

            foreach (var blog in blogs)
            {
                var model = Mapper.Map<Blog, BlogModel>(blog);
                model.SanitizeContent();
                blogList.Add(model);
            }

            var blogReturnObj = new
            {
                BlogList = blogList,
                CurrentPage = pageNumber,
                TotalBlogCount = blogs.TotalCount,
                WithCategory = new
                {
                    WithCategory = true,
                    CategoryName = categoryName.Replace('-', ' ')
                },
                WithTag = false
            };

            return Json(blogReturnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBlogsByUserId(int? pageNumber, int userId, string authorName)
        {
            var indexPage = pageNumber - 1 ?? 0;
            var blogs = _blogService.GetAllBlogPostsByUserId(indexPage, 10, userId);
            var blogList = new List<BlogModel>();

            foreach (var blog in blogs)
            {
                var model = Mapper.Map<Blog, BlogModel>(blog);
                model.SanitizeContent();
                blogList.Add(model);
            }

            var blogReturnObj = new
            {
                BlogList = blogList,
                CurrentPage = pageNumber,
                TotalBlogCount = blogs.TotalCount,
                WithCategory = false,
                WithTag = false,
                IsUserPage = new
                {
                    WithUser = true,
                    UserId = userId,
                    AuthorName = authorName
                },
            };

            return Json(blogReturnObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPromotedBlog()
        {
            var blog = _blogService.GetPromotedBlog();
            var model = Mapper.Map<Blog, BlogModel>(blog);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PromoteBlog(int id)
        {
            var isSuccess = _blogService.PromoteBlog(id);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ApproveBlog(int id, bool approvalStatus)
        {
            var isSuccess = _blogService.ApproveBlog(id, approvalStatus);
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSuggestedBlogs(int numOfBlogs, int? blogId)
        {
            blogId = blogId ?? null;
            var suggestedBlogs = _blogService.GetSuggestedBlogs(numOfBlogs, blogId);
            var blogList = new List<BlogModel>();
            foreach (var blog in suggestedBlogs)
            {
                var model = Mapper.Map<Blog, BlogModel>(blog);
                model.SanitizeContent();
                blogList.Add(model);
            }

            return Json(blogList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateBlog(BlogModel model)
        {
            if (model == null) return Json(false, JsonRequestBehavior.AllowGet);

            var blog = Mapper.Map<BlogModel, Blog>(model);
            blog.UserID = _userService.GetLoggedUser().ID;


            var isSuccess = _blogService.UpdateBlog(blog);

            var returnBlogDic = new
            {
                blog = new
                {
                    Title = blog.Title,
                    ID = blog.ID
                },
                isSuccess = isSuccess
            };

            if (model.Keywords != null)
            {
                var listOfKeywords = model.Keywords.Select(keyword => new Keyword()
                {
                    Keywords = keyword
                }).ToList();
                _blogService.UpdateKeywords(listOfKeywords, blog);
            }

            _categoryService.UpdateBlogCategory(blog.BlogCategories.FirstOrDefault(), blog.ID);

            #region Update VideoLinks
            if (model.VideoLink != null)
            {
                var videoLink = new BlogVideo()
                {
                    BlogID = model.ID,
                    VideoLink = model.VideoLink
                };
                _videoService.UpdateVideoLink(videoLink, model.ID);

            }
            #endregion

            #region update imageLink
            if (model.ImageLinks != null)
                _imageService.UpdateBlogImageLink(new Imagelink()
                {
                    FullImageLink = model.ImageLinks.FullImageLink,
                    ThumbnailImageLink = model.ImageLinks.ThumbnailImageLink
                }, model.ID);



            return Json(returnBlogDic, JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult UploadBlogImage(BlogModel model)
        {
            var imagePath = _imageService.UploadImage(Request, "blog");
            return Json(imagePath, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertBlog(BlogModel blog)
        {
            var isSuccess = false;
            var newBlog = Mapper.Map<BlogModel, Blog>(blog);
            newBlog.UserID = _userService.GetLoggedUser().ID;


            var returnBlog = _blogService.InsertBlog(newBlog);

            if (blog.Keywords != null)
            {
                var listOfKeywords = blog.Keywords.Select(keyword => new Keyword()
                {
                    Keywords = keyword
                }).ToList();
                _blogService.UpdateKeywords(listOfKeywords, newBlog);
            }

            #region Update VideoLinks
            if (blog.VideoLink != null)
            {
                var videoLink = new BlogVideo()
                {
                    BlogID = returnBlog.ID,
                    VideoLink = blog.VideoLink
                };
                _videoService.UpdateVideoLink(videoLink, returnBlog.ID);

            }
            #endregion

            if (blog.ImageLinks != null)
                _imageService.UpdateBlogImageLink(new Imagelink()
                {
                    FullImageLink = blog.ImageLinks.FullImageLink,
                    ThumbnailImageLink = blog.ImageLinks.ThumbnailImageLink
                }, returnBlog.ID);


            if (returnBlog != null)
                isSuccess = true;
            return Json(isSuccess, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTags()
        {
            var keywords = _blogService.GetKeywords().ToList();
            var listOfKeywords = keywords.Select(Mapper.DynamicMap<Keyword, KeywordModel>).ToList();
            return Json(listOfKeywords, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Utilities

        private bool IsOwner(int userId)
        {
            return _userService.GetLoggedUser().ID == userId;
        }

        #endregion
    }
}