using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Blog.Services.Image;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Website.Models;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Website.Controllers
{
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IEmailService _emailService;
        private readonly IEncryptionService _encryptionService;
        private readonly IUserService _userService;
        private readonly IImageService _imageService;
       
        public UserController(IRepository<User> userRepository,
            IEmailService emailService,
            IEncryptionService encryptionService,
            IUserService userService,
            IImageService imageService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _encryptionService = encryptionService;
            _userService = userService;
            _imageService = imageService;
        }

        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public User GetLoggedInUser()
        {
            var loggedInUser = _userService.GetLoggedUser();
            return loggedInUser;
        }

        public ActionResult AuthenticateUser(string email, string password)
        {
            var isAuthenticated = _userService.AuthenticateUser(email, password);
            if (isAuthenticated)
            {
                FormsAuthentication.SetAuthCookie(email, false);
            }
            return Json(isAuthenticated, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePassword(string email, string password)
        {
            var isSuccessfull = _userService.ChangePassword(email, password);
            return Json(isSuccessfull, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCurrentUserProfile()
        {
            var model = GetLoggedInUser();
            var user = Mapper.Map<User, UserModel>(model);
            user.BlogPostedCount = _userService.GetBlogCount(user.ID);
            user.Password = null;
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IsEmailUnique(string email)
        {
            var isEmailUnique = _emailService.IsEmailUnique(email);
            return Json(isEmailUnique, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllUsers()
        {
            var model = _userService.GetAllUsers();
            var userList = new List<UserModel>();

            foreach (var user in model)
            {
                var userObj = Mapper.Map<User, UserModel>(user);
                user.Password = null;
                userList.Add(userObj);
            }
           
            return Json(userList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetUser(int id)
        {
            var model = _userService.GetUser(id);
            var user = Mapper.Map<User, UserModel>(model);
            user.Password = null;
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddUser(UserModel model)
        {
            if (model == null) return Json(false, JsonRequestBehavior.AllowGet);

            var user = Mapper.Map<UserModel, User>(model);
            var isAdded = _userService.AddUser(user);
            if (isAdded) FormsAuthentication.SetAuthCookie(user.Email, false);

            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadUserImage()
        {
            var imagePath = _imageService.UploadImage(Request, "user");
            return Json(imagePath, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateUserProfile(UserModel model)
        {
            if (model == null) return Json(false, JsonRequestBehavior.AllowGet);

            if (model.ImageLinks != null)
                _imageService.UpdateUserImage(new Imagelink()
                {
                    FullImageLink = model.ImageLinks.FullImageLink,
                    ThumbnailImageLink = model.ImageLinks.ThumbnailImageLink
                }, model.ID);

            var user = Mapper.Map<UserModel, User>(model);
            var isUpdated = _userService.UpdateUser(user);

        

            if (isUpdated) FormsAuthentication.SetAuthCookie(user.Email, false);

            return Json(isUpdated, JsonRequestBehavior.AllowGet);
        }
    }
}