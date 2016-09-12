using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Premiumstress.Blog.Services.Blogs;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Blog.Services.Image;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Services.Video;
using Premiumstress.Blog.Website.Controllers;
using Premiumstress.Blog.Website.Models;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Data.Test.Services.Blog
{
    using Core.Domain.Blog;
    using Core.Domain;

    [TestClass]
    public class BlogControllerTest
    {
        private BlogController _blogController;
        private IRepository<Blog> _blogRepository;
        private IRepository<Keyword> _keywordRepository;
        private IRepository<Category> _categoryRepository;
        private IRepository<BlogCategory> _blogCategoryRepository;
        private IRepository<Imagelink> _imageLinkRepository;
        private IRepository<User> _userRepository;
        private IRepository<BlogVideo> _blogVideoRepository;

        private IBlogService _blogService;
        private ICategoryService _categoryService;
        private IImageService _imageService;
        private IUserService _userService;
        private IVideoService _videoService;
        private IEncryptionService _encryptionService;
        private IEmailService _emailService;
        private IUnitOfWork _unitOfWork;

        private DbContext _context;
        [TestInitialize]
        public void Initialize()
        {
            _context = new PremiumStressContext();
            _blogRepository = new Repository<Blog>(_context);
            _keywordRepository = new Repository<Keyword>(_context);
            _userRepository = new Repository<User>(_context);
            _imageLinkRepository = new Repository<Imagelink>(_context);
            _unitOfWork = new UnitOfWork();
            _blogService = new BlogService(_blogRepository, _keywordRepository, _unitOfWork);
            _categoryService = new CategoryService(_categoryRepository,_blogCategoryRepository);
            _imageService = new ImageService(_imageLinkRepository,_userRepository);
            _encryptionService = new EncryptionService();
            _emailService = new EmailService(_userRepository);
            _userService = new UserService(_userRepository,_encryptionService, _emailService,_blogRepository);
            _blogVideoRepository = new Repository<BlogVideo>(_context);
            _blogController = new BlogController(_blogService,
                _categoryService, _imageService, _userService,_videoService);
        }

        [TestMethod]
        public void Query_All_Active_Blog_Posts_Controller()
        {
            var allBlogs = _blogController.GetBlogs(1,"blog","viewcount","desc");
            Console.WriteLine(allBlogs);
        }

        [TestMethod]
        public void Query_All_Tags()
        {
            var allBlogs = _blogController.GetTags();
            Console.WriteLine(allBlogs);
        }

        [TestMethod]
        public void Approve_Blog()
        {
            var isSuccess = _blogController.ApproveBlog(41,true);
            Console.WriteLine(isSuccess);
        }

        [TestMethod]
        public void Query_Insert_Blog()
        {
            var blog = new BlogModel()
            {
                Category = new CategoryModel()
                {
                    ID = 1,
                    Name = "Web Development"
                },
                Content = "Forth will",
                Title = "This is from BlogControllerTest again",
                ImageLinks = new ImageLinkModel()
                {
                    FullImageLink = "/Images/BlogImages/f930aa0b-c776-45a2-a9c7-f7dd7290851d.PNG",
                    ThumbnailImageLink = "/Images/BlogImages/Thumbnails/f930aa0b-c776-45a2-a9c7-f7dd7290851d.PNG"
                },
                Keywords = new List<string>()
                {
                    "bag",
                    "another"
                },
                UserID = 1
            };
            var allBlogs = _blogController.InsertBlog(blog);
            Console.WriteLine(allBlogs);
        }
    }
}
