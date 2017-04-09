using System.Web;
using System.Web.Optimization;

namespace Zuul.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/forum").Include(
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/linkify.js",
                        "~/Scripts/linkify-jquery.js",
                        "~/Scripts/moment.js",
                        "~/Scripts/format.js",
                        "~/Scripts/zuul/setup-moment.js",
                        "~/Scripts/zuul/ko-extensions.js",
                        "~/Scripts/zuul/namespace.js",
                        "~/Scripts/zuul/constants.js",
                        "~/Scripts/zuul/ForumService.js",
                        "~/Scripts/zuul/ForumViewModel.js",
                        "~/Scripts/zuul/Reply.js",
                        "~/Scripts/zuul/Thread.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            #if !DEBUG
            BundleTable.EnableOptimizations = true;
            #endif
        }
    }
}
