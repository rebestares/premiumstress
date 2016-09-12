using System.Text;
using System.Web.Mvc;
using Premiumstress.Blog.Services.Email;
using Premiumstress.Blog.Services.Encryption;
using Premiumstress.Blog.Services.Image;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Website.Controllers;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Website.Html
{
    using Core.Domain.Blog;

    public static class  HtmlHelpers
    {
        private static PremiumStressContext _context = new PremiumStressContext();
        private static readonly IRepository<User> userRepository = new Repository<User>(_context);
        private static readonly IRepository<Imagelink> imageRepository = new Repository<Imagelink>(_context);
        private static readonly IRepository<Blog> _blogRepository = new Repository<Blog>(_context);
        private static readonly IEmailService emailService = new EmailService(userRepository);
        private static readonly IEncryptionService encryptionService = new EncryptionService();
        private static readonly IImageService imageService = new ImageService(imageRepository,userRepository);
        private static readonly IUserService userService = new UserService(userRepository, encryptionService,
            emailService, _blogRepository);

        public static MvcHtmlString NavigationLink(this HtmlHelper htmlHelper,
            string linkText,
            string actionName,
            string controllerName
            )
        {
            string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            TagBuilder navigation = new TagBuilder("li");
            TagBuilder link = new TagBuilder("a");

           
            UserController loggedUser = new UserController(userRepository, emailService, encryptionService, userService, imageService);
            var loggedInUser = loggedUser.GetLoggedInUser();

            //To check if user is logged in
            if (loggedInUser != null)
            {
                if (loggedInUser.ID == 0)
                {
                    if (actionName == "Signout" && loggedInUser.ID == 0)
                    {
                        //actionName = "Index";
                        //linkText = "Log In";
                        //link.Attributes.Add("data-reveal-id", "myModal");
                        return MvcHtmlString.Create(navigation.ToString(TagRenderMode.Normal));
                    }

                    if (controllerName.ToLower() == "admin")
                    {
                        return MvcHtmlString.Create(string.Empty);
                    }
                }
            }

            //Add this attribute for angular js routing to be directed to a full page
            link.Attributes.Add("target", "_self");
            if (controllerName == "blog")
            {
                link.Attributes.Add("href", "/");
            }
            else
            {
                link.Attributes.Add("href", "/" + controllerName);
            }

            TagBuilder icon = new TagBuilder("i");
            icon.AddCssClass(GetCssIcons(linkText));

            //Add the icon tag together with a br tag and the linkText
            link.InnerHtml = icon.ToString(TagRenderMode.Normal) + new TagBuilder("br").ToString(TagRenderMode.StartTag) + linkText;
            navigation.InnerHtml = link.ToString(TagRenderMode.Normal);

            if (controllerName == currentController.ToLower())
            {
                navigation.Attributes.Add("class", "active text-center");
                return MvcHtmlString.Create(navigation.ToString(TagRenderMode.Normal));
            }
            navigation.Attributes.Add("class", "text-center");

            return MvcHtmlString.Create(navigation.ToString(TagRenderMode.Normal));
        }

        private static string GetCssIcons(string linkText)
        {
            var iconClass = new StringBuilder("fa ");

            //To determine the icons for the link
            switch (linkText)
            {
                case "Home":
                    iconClass.Append("fa-home");
                    break;
                case "About":
                    iconClass.Append("fa-user");
                    break;
                case "Contact":
                    iconClass.Append("fa-envelope-o");
                    break;
                case "Settings":
                    iconClass.Append("fa-wrench");
                    break;
                case "Logout":
                    iconClass.Append("fa-power-off");
                    break;
            }

            return iconClass.ToString();
        }
    }
}