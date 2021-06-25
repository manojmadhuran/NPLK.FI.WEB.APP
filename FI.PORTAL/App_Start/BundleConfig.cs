using System.Web;
using System.Web.Optimization;

namespace FI.PORTAL
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862       
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/Content/scripts").Include(
                    "~/Content/plugins/jquery/jquery.min.js",
                    "~/Content/plugins/bootstrap/js/bootstrap.bundle.min.js",

                    "~/Content/plugins/chart.js/Chart.min.js",

                    "~/Content/plugins/sparklines/sparkline.js",

                    "~/Content/plugins/jqvmap/jquery.vmap.min.js",
                    "~/Content/plugins/jqvmap/maps/jquery.vmap.usa.js",
                    "~/Content/plugins/jquery-knob/jquery.knob.min.js",
                    "~/Content/plugins/moment/moment.min.js",
                    "~/Content/plugins/daterangepicker/daterangepicker.js",

                    "~/Content/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js",

                    "~/Content/plugins/summernote/summernote-bs4.min.js",

                    "~/Content/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js",

                    "~/Content/dist/js/adminlte.js",

                    "~/Content/dist/js/demo.js",

                     "~/Content/plugins/select2/js/select2.full.min.js",

                    "~/Content/plugins/bootstrap-switch/js/bootstrap-switch.min.js",

                    "~/Content/plugins/datatables/jquery.dataTables.js",
                    "~/Content/plugins/datatables-bs4/js/dataTables.bootstrap4.min.js",
                    "~/Content/validatity.js",
                    "~/Content/dist/js/toastr.min.js",
                    "~/Content/plugins/bs-custom-file-input/bs-custom-file-input.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/plugins/fontawesome-free/css/all.min.css",
                     "~/Content/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css",
                     "~/Content/plugins/icheck-bootstrap/icheck-bootstrap.min.css",
                     "~/Content/plugins/jqvmap/jqvmap.min.css",
                     "~/Content/dist/css/adminlte.min.css",
                     "~/Content/plugins/overlayScrollbars/css/OverlayScrollbars.min.css",
                     "~/Content/plugins/daterangepicker/daterangepicker.css",
                     "~/Content/plugins/summernote/summernote-bs4.css",
                     "~/Content/plugins/select2/css/select2.min.css",
                     "~/Content/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css",
                     "~/Content/plugins/datatables-bs4/css/dataTables.bootstrap4.css",
                     "~/Content/dist/css/toastr.min.css"
                      ));

            bundles.Add(new ScriptBundle("~/Scripts/Controller").Include(
                    "~/Scripts/ControllerScripts/initiation.js",
                    "~/Scripts/ControllerScripts/comments.js",            
                    "~/Scripts/ControllerScripts/userlogin.js"               
                              
                                       
                ));
     
        }
    }
}
