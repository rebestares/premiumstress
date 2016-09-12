using System.Data.Entity;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Blog.Services.User;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Data.Test.Services.User
{
    using Core.Domain;
    using Core.Domain.Blog;
    [TestClass]
    public class UserServiceTest
    {
        private Repository<User> _userRepository;
        private Repository<Blog> _blogRepository;
        private IUserService _userService;
        private IEncryptionService _encryptionService;
        private IEmailService _emailService;
        private DbContext _context;

        [TestInitialize]
        public void Initialize()
        {
            _context = new PremiumStressContext();
            _userRepository = new Repository<User>(_context);
            _blogRepository = new Repository<Blog>(_context);
            _encryptionService = new EncryptionService();
            _emailService = new EmailService(_userRepository);
            _userService = new UserService(_userRepository, _encryptionService, _emailService,_blogRepository);
        }

        [TestMethod]
        public void Query_Get_User_By_ID()
        {
            var user = _userService.GetUser(1);

            Trace.TraceInformation("User name: {0} {1}", user.FirstName,user.LastName);
        }        
        
        [TestMethod]
        public void Query_AuthenticatedUser()
        {
            var user = _userService.GetUser(1);
            var isAuthenticated = _userService.AuthenticateUser(user.Email, user.Password);
            Trace.TraceInformation("Is Authenticated? {0}", isAuthenticated);
        }

        [TestMethod]
        public void Query_GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            foreach (var user in users)
            {
                Trace.TraceInformation("User:{0},{1}", user.FirstName,user.LastName);
            }
            
        }
    }
}
