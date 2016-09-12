using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Premiumstress.Blog.Services.Category;
using Premiumstress.Blog.Services.Contact;
using Premiumstress.Blog.Services.Tag;
using Premiumstress.Blog.Services.User;
using Premiumstress.Blog.Website.Models;
using Premiumstress.Core.Domain;
using Premiumstress.Data.Blog;

namespace Premiumstress.Blog.Website.Controllers
{
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IContactService _contactService;
        private readonly ITagService _tagService;
        private readonly IRepository<User> _useRepository;
        private readonly IUserService _userService;

        public AdminController(
            IRepository<User> useRepository,
            IUserService userService,
            ICategoryService categoryService,
            IContactService contactService,
            ITagService tagService
            )
        {
            _useRepository = useRepository;
            _userService = userService;
            _categoryService = categoryService;
            _contactService = contactService;
            _tagService = tagService;
        }

        // GET: Admin
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Setting");

            return View();
        }

        [Authorize]
        public ActionResult Setting()
        {
            return View();
        }

        public ActionResult UpdateCategoryList(List<CategoryModel> categories)
        {
            var model = categories.Select(Mapper.Map<CategoryModel, Category>).ToList();
            var isUpdated = _categoryService.UpdateCategory(model);
            return Json(isUpdated, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InsertCategory(CategoryModel category)
        {
            var model = Mapper.Map<CategoryModel, Category>(category);
            var isAdded = _categoryService.InsertCategory(model);
            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DeleteCategory(CategoryModel category)
        {
            var model = Mapper.Map<CategoryModel, Category>(category);
            var isAdded = _categoryService.DeleteCategory(model);
            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCategories()
        {
            var categories = _categoryService.GetCategoriesFor("blog");
            var categoryList = categories.Select(Mapper.Map<Category,CategoryModel>).ToList();

            return Json(categoryList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendToAdmin(EmailModel email)
        {
            var model = Mapper.Map<EmailModel, Email>(email);
            var isAdded = _contactService.SendToAdmin(model);
            return Json(isAdded, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTags()
        {
            var keywords = _tagService.GetAllKeywords();
            var keywordList = keywords.Select(Mapper.Map<Keyword, KeywordModel>).ToList();

            return Json(keywordList, JsonRequestBehavior.AllowGet);
        }



        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            //var user = Globals.Methods.GetCurrentLoggedInUser();
            //var isSuccess = Globals.Methods.SetUserLogStatus(user.Email, false);

            return RedirectToAction("Index", "Blog");
        }
    }
}