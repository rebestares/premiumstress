using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Premiumstress.Blog.Services.User
{
    using Core.Domain;
    
    public interface IUserService
    {
        bool AddUser(User user);
        bool AuthenticateUser(string email, string password);
        bool ChangePassword(string email, string password);
        bool UpdateUser(User user);
        User GetUser(int id);
        List<User> GetAllUsers();
        int GetBlogCount(int id);
        User GetLoggedUser();
    }
}
