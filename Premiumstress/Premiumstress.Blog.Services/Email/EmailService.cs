using System.Linq;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Services.Email
{
    using Core.Domain;

    public class EmailService : IEmailService
    {
        private readonly IRepository<User> _userRepository;

        public EmailService(IRepository<User> userRepository )
        {
            _userRepository = userRepository;
        }

        public bool IsEmailUnique(string email)
        {
            var query = _userRepository.Table;
            var isUnique = !query.Any(a => a.Email == email);
            
            return isUnique;
        }
    }
}