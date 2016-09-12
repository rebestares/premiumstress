using System.Web.Mvc;
using System.Web.Routing;

namespace Premiumstress.Blog.Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            #region Pages

            routes.MapRoute("Admin", "admin",
                new { controller = "Admin", action = "Index" }
                );

            routes.MapRoute("AdminSetting", "settings/{*anything}",
                new { controller = "Admin", action = "Setting" }
                );

            routes.MapRoute("AdminSignout", "signout",
                new { controller = "Admin", action = "Signout" }
                );

            routes.MapRoute("About", "about",
                new { controller = "About", action = "Index" }
                );

            routes.MapRoute("Contact", "contact",
                new { controller = "Contact", action = "Index" }
                );

            routes.MapRoute("GetStaffProfiles", "contact/getstaffprofiles",
                new { controller = "Contact", action = "Getstaffprofiles" }
                );

            routes.MapRoute("EditBlog", "edit/{id}/{title}",
                new { controller = "Blog", action = "Edit", title = UrlParameter.Optional, id = UrlParameter.Optional }
                );

            routes.MapRoute("AddNewBlog", "blog/add",
                new { controller = "Blog", action = "Add" }
                );

            #endregion

            #region API

            #region Blog Routes

            routes.MapRoute("GetBlogs", "blog/getblogs",
                new { controller = "Blog", action = "GetBlogs" }
                );

            routes.MapRoute("GetFeatured", "blog/getfeatured",
                new { controller = "Blog", action = "GetFeatured" }
                );

            routes.MapRoute("FindBlog", "blog/findblogs",
                new { controller = "Blog", action = "FindBlogs" }
                );

            routes.MapRoute("GetBlogsByCategory", "blog/getblogsbycategory",
                new { controller = "Blog", action = "GetBlogsByCategory" }
                );

            routes.MapRoute("GetBlogsByTag", "blog/getblogsbytag",
                new { controller = "Blog", action = "GetBlogsByTag" }
                );

            routes.MapRoute("GetBlogsByUserId", "blog/getblogsbyuserid",
                new { controller = "Blog", action = "GetBlogsByUserId" }
                );

            routes.MapRoute("GetPromotedBlog", "blog/getpromotedblog",
                new { controller = "Blog", action = "GetPromotedBlog" }
                );

            routes.MapRoute("GetSingleBlog", "blog/getsingleblog",
                new { controller = "Blog", action = "GetSingleBlog" }
                );

            routes.MapRoute("GetSuggestedBlogs", "blog/getsuggestedblogs",
                new { controller = "Blog", action = "GetSuggestedBlogs" }
                );

            routes.MapRoute("UploadBlogImage", "blog/uploadblogimage",
                new { controller = "Blog", action = "UploadBlogImage" }
                );

            routes.MapRoute("UpdateBlog", "blog/updateblog",
                new { controller = "Blog", action = "UpdateBlog" }
                );

            routes.MapRoute("InsertBlog", "blog/insertblog",
                new { controller = "Blog", action = "InsertBlog" }
                );

            routes.MapRoute("DeleteBlog", "blog/deleteblog",
                new { controller = "Blog", action = "DeleteBlog" }
                );

            routes.MapRoute("BlogSingle", "Article/{id}/{blogTitle}",
                new
                {
                    controller = "Blog",
                    action = "Article",
                    blogTitle = UrlParameter.Optional,
                    id = UrlParameter.Optional
                }
                );


            routes.MapRoute("GetTags", "blog/gettags",
                new { controller = "Blog", action = "GetTags" }
                );

            routes.MapRoute("PromoteBlog", "blog/promoteblog",
                new { controller = "Blog", action = "PromoteBlog" }
                );

            routes.MapRoute("ApproveBlog", "blog/approveblog",
                new { controller = "Blog", action = "ApproveBlog" }
                );

            #endregion

            #region User Routes

            routes.MapRoute("GetCurrentUserProfile", "user/GetCurrentUserProfile",
                new { controller = "User", action = "GetCurrentUserProfile" }
                );

            routes.MapRoute("GetAllUsers", "user/GetUsers",
              new { controller = "User", action = "GetAllUsers" }
              );

            routes.MapRoute("AuthenticateUser", "user/authenticateuser",
                new { controller = "User", action = "AuthenticateUser" }
                );

            routes.MapRoute("AddUser", "user/adduser",
               new { controller = "User", action = "AddUser" }
               );

            routes.MapRoute("ChangePassword", "user/changepassword",
                new { controller = "User", action = "ChangePassword" }
                );

            routes.MapRoute("UploadUserImage", "user/uploaduserimage",
                new { controller = "User", action = "UploadUserImage" }
                );

            routes.MapRoute("UpdateUserProfile", "user/updateuserprofile",
                new { controller = "User", action = "UpdateUserProfile" }
                );

            routes.MapRoute("GetUser", "user/getuser",
                new { controller = "User", action = "GetUser" }
                );

            routes.MapRoute("IsEmailUnique", "user/isemailunique",
            new { controller = "User", action = "IsEmailUnique" }
            );

            #endregion

            #region Category Routes

            routes.MapRoute("GetCategories", "admin/getcategories",
                new { controller = "Admin", action = "GetCategories" }
                );

            routes.MapRoute("UpdateCategoryList", "admin/updatecategorylist",
                new { controller = "Admin", action = "UpdateCategoryList" }
                );

            routes.MapRoute("InsertCategory", "admin/insertcategory",
                new { controller = "Admin", action = "InsertCategory" }
                );
            routes.MapRoute("DeleteCategory", "admin/deletecategory",
                new { controller = "Admin", action = "DeleteCategory" }
                );

            #endregion
            routes.MapRoute("GetAllTags", "admin/gettags",
               new { controller = "Admin", action = "GetTags" }
               );
            #region Tag Routes
            #endregion

            #region Contact Routes

            routes.MapRoute("SendToAdmin", "admin/sendtoadmin",
                new { controller = "Admin", action = "SendToAdmin" }
                );

            #endregion

            #endregion

            //Route for angular JS to make sure blog index is served always
            routes.MapRoute("Default", "{*anything}",
                new { controller = "Blog", action = "Index" }
                );



            routes.AppendTrailingSlash = false;
        }
    }
}