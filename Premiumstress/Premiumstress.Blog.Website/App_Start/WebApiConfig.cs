using System.Web.Http;

namespace Premiumstress.Blog.Website
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            //config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}/{id}", new {id = RouteParameter.Optional}
            //    );

            //config.Routes.MapHttpRoute("DefaultApi2", "api/{controller}/{action}/{id}",
            //    new {id = RouteParameter.Optional}
            //    );

            //config.Routes.MapHttpRoute("ApiByName", "{controller}/{action}/{name}", null, new {name = @"^[a-z]+$"}
            //    );

            //config.Routes.MapHttpRoute("ApiByAction", "{controller}/{action}", new {action = "Get"}
            //    );
        }
    }
}