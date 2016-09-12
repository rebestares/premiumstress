namespace Premiumstress.Blog.Services.Email
{
    public interface IEmailService
    {
        bool IsEmailUnique(string email);
    }
}
