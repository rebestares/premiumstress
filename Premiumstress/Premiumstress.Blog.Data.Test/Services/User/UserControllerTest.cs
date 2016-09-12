using System;
using System.Data.Entity;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Blog.Services.Image;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Website.Controllers;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Data.Test.Services.User
{
    using Core.Domain.Blog;
    using Core.Domain;

    [TestClass]
    public class UserControllerTest
    {
        private UserController _userController;
        private IRepository<User> _userRepository;
        private IRepository<Blog> _blogRepository;
        private IRepository<Imagelink> _imageLinkRepository;
        private IEmailService _emailService;
        private IEncryptionService _encryptionService;
        private IUserService _userService;
        private IImageService _imageService;
        private DbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new PremiumStressContext();
            _userRepository = new Repository<User>(_context);
            _blogRepository = new Repository<Blog>(_context);
            _imageLinkRepository = new Repository<Imagelink>(_context);
            _imageService = new ImageService(_imageLinkRepository,_userRepository);
            _encryptionService = new EncryptionService();
            _emailService = new EmailService(_userRepository);
            _userService = new UserService(_userRepository, _encryptionService, _emailService, _blogRepository);
            _userController = new UserController(_userRepository,_emailService,_encryptionService,_userService,_imageService);
        }

        [TestMethod]
        public void Query_All_Users()
        {
            var users = _userController.GetAllUsers() as JsonResult;
            Console.WriteLine(users);
        }

        [TestMethod]
        public void Query_Single_User()
        {
            var users = _userController.GetUser(1) as JsonResult;

            dynamic data = users?.Data;
        }

    }
}
