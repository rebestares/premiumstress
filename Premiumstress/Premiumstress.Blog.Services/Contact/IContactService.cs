using System.Collections.Generic;
namespace Premiumstress.Blog.Services.Contact
{
    using Core.Domain;

    public interface IContactService
    {
        IEnumerable<Core.Domain.User> GetStaffProfiles();
        bool SendToAdmin(Email email);
    }
}