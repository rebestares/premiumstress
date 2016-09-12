using System.Web.Optimization;

namespace Premiumstress.Blog.Website
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/angular-touch.js",
                "~/Scripts/foundation/fastclick.js",
                "~/Scripts/foundation/foundation.js",
                "~/Scripts/foundation/foundation.reveal.js",
                "~/Scripts/angularLoadingBar/loading-bar.js",
                "~/Scripts/textAngular/textAngular-rangy.min.js",
                "~/Scripts/textAngular/textAngular.js",
                "~/Scripts/textAngular/textAngular-sanitize.js",
                "~/Scripts/textAngular/textAngularSetup.js",
                "~/Scripts/foundation/fastclick.*"
                ).IncludeDirectory("~/Scripts/Angular", "*.js", true));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            //    "~/Scripts/jquery-ui-{version}.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //    "~/Scripts/jquery.unobtrusive*",
            //    "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            //// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //    "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
                "~/Content/MainSite.css",
                "~/Content/textAngular/textAngular.css",
                "~/Content/angularLoadingBar/loading-bar.css",
                "~/Content/webicons/webicons.css",
                "~/Content/foundation-icons/foundation-icons.css",
                "~/Content/font-awesome/font-awesome.css"));
        }
    }
}