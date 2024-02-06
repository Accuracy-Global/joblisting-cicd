using System.Web;
using System.Web.Optimization;

namespace JobPortal.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;
            //bundles.Add(new ScriptBundle("~/bundles/jquery", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-2.1.4.min.js").Include(
            //    "~/Scripts/jquery-2.1.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
               "~/Scripts/jquery-2.1.4.min.js",
               "~/Scripts/jquery.validate.min.js",
               "~/Scripts/jquery.validate.unobtrusive.min.js"
               ));


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryall").Include(
                "~/Scripts/jquery-2.1.4.min.js",
                "~/Scripts/jquery.validate.min.js",
                "~/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Scripts/jquery.signalR-2.2.0.min.js",
                "~/Scripts/jquery.canvasjs.min.js",
                "~/Scripts/jquery.cookie.js",
                "~/Scripts/jquery.dataTables.min.js",
                "~/Scripts/jquery.quick.pagination.min.js",
                "~/Scripts/jquery.share.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/spg").Include(
                "~/Scripts/jquery-2.1.4.min.js",
                "~/Scripts/bootstrap.min.js",
                 "~/Scripts/bootstrap-datetimepicker.min.js",
                "~/Scripts/toastr.min.js",
                "~/Scripts/jquery.tmpl.min.js",
                "~/Scripts/JobPortal.js"
              ));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
            //    "~/Scripts/JobPortal.js"));

            bundles.Add(new ScriptBundle("~/bundles/jScripts").Include(
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/bootstrap-datetimepicker.min.js",
                "~/Scripts/dataTables.bootstrap.js",
                "~/Scripts/dataTables.responsive.js",
                "~/Scripts/canvasjs.min.js",
                "~/Scripts/respond.min.js",
                "~/Scripts/toastr.min.js",
                "~/Scripts/JobPortal.js"));

            bundles.Add(new ScriptBundle("~/bundles/lScripts").Include(
                 "~/Scripts/jquery-2.1.4.min.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/tinymce/tinymce.min.js").Include(
                "~/Scripts/tinymce/tinymce.min.js"));

            bundles.Add
            (
                new StyleBundle("~/Content/css")
                .Include
                (
                    "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap-datetimepicker.css",
                    "~/Content/dataTables.bootstrap.css",
                    "~/Content/toastr.min.css"
                )
            );

            bundles.Add(new StyleBundle("~/bundles/spgcss").Include(
                    "~/Content/bootstrap.min.css",
                    "~/Content/bootstrap-datetimepicker.css",
                    "~/Content/dataTables.bootstrap.css",
                     "~/Content/toastr.min.css"
             ));

            bundles.Add
           (
               new StyleBundle("~/Content/bcss")
               .Include
               (
                   "~/Content/bootstrap.min.css",
                   "~/Content/bootstrap-datetimepicker.css",
                   "~/Content/toastr.min.css"
               )
           );

            bundles.Add
                (
                   new StyleBundle("~/Content/bootstrapcss")
                    .Include
                    (
                        "~/Content/bootstrap.min.css"
                    )
                );

            bundles.Add(new StyleBundle("~/content/joblisting/resume")
                .Include(
                "~/Content/font-awesome.min.css",
                "~/Content/bootstrap.min.css",
                "~/Content/bootstrap-datetimepicker.min.css",
                "~/Content/toastr.min.css",
                "~/Content/joblisting/resume.css"));

            // SignalR bundle
            bundles.Add(new ScriptBundle("~/bundles/SignalR").Include(
                        "~/Scripts/jquery.signalR-{version}.js"));

            //BundleTable.EnableOptimizations = true;
        }
    }
}