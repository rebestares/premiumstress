using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.User
{
    using Core.Domain;
    using Core.Domain.Blog;
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IEmailService _emailService;
        private readonly IRepository<Blog> _blogRepository;

        public UserService(
            IRepository<User> userRepository,
            IEncryptionService encryptionService,
            IEmailService emailService,
            IRepository<Blog> blogRepository)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _emailService = emailService;
            _blogRepository = blogRepository;
        }

        #region Methods
        /// <summary>
        /// Add a new user
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns>If successfully added</returns>
        public bool AddUser(User user)
        {
            var isSuccess = false;

            var query = _userRepository.Table;

            if (user.Password != null)
                user.Password = _encryptionService.ConvertoToMd5(user.Password);

            if (!_emailService.IsEmailUnique(user.Email))
                return false;

            var lastEntry = query.OrderByDescending(u => u.ID).FirstOrDefault();

            user.ID = lastEntry?.ID + 1 ?? 1;
            user.DateJoined = DateTime.Now;
            isSuccess = (_userRepository.Insert(user));

            return isSuccess;
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="user">user object</param>
        /// <returns>If successfully updated</returns>
        public bool UpdateUser(User user)
        {
            var currentUser = (from userObj in _userRepository.Table
                               where userObj.ID == user.ID
                               select userObj).SingleOrDefault();

            user.Password = currentUser?.Password;

            var isSuccess = (_userRepository.Update(user));
            return isSuccess;
        }

        /// <summary>
        /// Check if user is authenticated
        /// </summary>
        /// <param name="email">User Email</param>
        ///  /// <param name="password">User Password</param>
        /// <returns>If user is authenticated</returns>
        public bool AuthenticateUser(string email, string password)
        {
            var isAuthenticated = false;

            var query = _userRepository.Table;
            var user = query.FirstOrDefault(a => a.Email == email);

            //If user is empty it is not authenticated
            if (user == null) return false;

            //If password is empty it is not authenticated
            if (string.IsNullOrEmpty(password)) return false;

            password = _encryptionService.ConvertoToMd5(password);
            isAuthenticated = user.Password == password;

            return isAuthenticated;
        }

        /// <summary>
        /// Get a user by Id
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User</returns>
        public User GetUser(int id)
        {
            var query = _userRepository.Table;
            var user = query.FirstOrDefault(a => a.ID == id);
            return user;
        }

        /// <summary>
        /// Change the password of a user
        /// </summary>
        /// <param name="email">User Email</param>
        /// <param name="password">User Password</param>
        /// <returns>If changing of password is successful</returns>
        public bool ChangePassword(string email, string password)
        {
            var query = _userRepository.Table;
            var user = query.FirstOrDefault(a => a.Email == email);
            if (user == null) return false;

            user.Password = _encryptionService.ConvertoToMd5(password);

            _userRepository.Update(user);

            return true;
        }

        /// <summary>
        /// Get the blog count of the user
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>Number of blogs owned by the user</returns>
        public int GetBlogCount(int id)
        {
            var query = _blogRepository.Table;
            var blogCount = query.Count(a => a.UserID == id && (!a.IsDeleted.Value || !a.IsDeleted.HasValue));

            return blogCount;
        }

        /// <summary>
        /// Get the current logged user
        /// </summary>
        /// <returns>User</returns>
        public User GetLoggedUser()
        {
            var query = _userRepository.Table;
            var user = new User();
            var userEmail = System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.User.Identity.Name : string.Empty;

            if (!string.IsNullOrEmpty(userEmail))
            {
                user = query.SingleOrDefault(a => a.Email == userEmail);
            }

            return user;
        }

        /// <summary>
        /// Get All Active users
        /// </summary>
        /// <returns>List of Users</returns>
        public List<User> GetAllUsers()
        {
            var users = (from user in _userRepository.Table
                where user.IsInactive.Value != true
                select user).ToList();

            return users;
        }

        #endregion

        #region Utilities
        #endregion
    }
}
