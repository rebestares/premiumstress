using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Premiumstress.Blog.Services.Contact;
using Premiumstress.Blog.Website.Models;
using Premiumstress.Core.Domain;

namespace Premiumstress.Blog.Website.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        // GET: Contact
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [System.Web.Http.ActionName("Get")]
        public ActionResult GetStaffProfiles()
        {
            var staffProfiles = _contactService.GetStaffProfiles();
            var listOfStaff = new List<UserModel>();
            foreach (var staffProfile in staffProfiles)
            {
                var staff = Mapper.Map<User, UserModel>(staffProfile);
                listOfStaff.Add(staff);
            }
           
            return Json(listOfStaff, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult SendEmailToAdmin(EmailClass email)
        //{
        //    var staffProfiles = contactObj.SendToAdmin(email);
        //    return Json(staffProfiles, JsonRequestBehavior.AllowGet);
        //}

        //protected void PrepareUserModel(User user, UserModel model)
        //{
        //    model.ID = user.ID;
        //    model.About = user.About;
        //    model.Birthday = user.Birthday;
        //}
    }
}