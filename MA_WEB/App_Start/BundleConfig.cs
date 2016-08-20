using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace MA_WEB
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui.min.js",
                "~/Scripts/finch.js",
                "~/Scripts/jquery.signalR-{version}.js",
                "~/Scripts/app/Helper/SignalRHelper.js"));

           /* bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.unobtrusive*",
                "~/Scripts/jquery.validate*"));*/

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                "~/Scripts/knockout-{version}.js",
                "~/Scripts/knockout.validation.js",
                "~/Scripts/knockout.simpleGrid.1.3.js",
                "~/Scripts/knockstrap.js",
                "~/Scripts/knockout.issueGrid.js",
                "~/Scripts/knockout-file-bindings.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/sammy-{version}.js",
                "~/Scripts/app/common.js",
                "~/Scripts/app/app.datamodel.js",
                "~/Scripts/app/app.viewmodel.js",
                "~/Scripts/app/home.viewmodel.js",
                "~/Scripts/app/_run.js",
                "~/Scripts/app/project.viewmodel.js",
                "~/Scripts/app/projects.viewmodel.js",
                "~/Scripts/app/addProject.viewmodel.js",
                 "~/Scripts/app/addRequirement.viewmodel.js",
                 "~/Scripts/app/requirement.viewmodel.js",
                 "~/Scripts/app/requirements.viewmodel.js",
                 "~/Scripts/app/requirementsByType.viewmodel.js",
                 "~/Scripts/app/addRequirementType.viewmodel.js",
                 "~/Scripts/app/userProfile.viewmodel.js",
                 "~/Scripts/attributeDescriptionTypes.js",
                 "~/Scripts/app/issue.viewmodel.js"
                 ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/bootstrap.min.js",
                "~/Scripts/respond.js",
                "~/Scripts/bootbox.js",
                "~/Scripts/bootbox.min.js",
                "~/Scripts/select2.min.js",
                "~/Scripts/jquery.dataTables.js",
                "~/Scripts/dataTables.bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/raphael").Include(
                    "~/Scripts/raphael-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-slider").Include(
                    "~/Scripts/plugins/bootstrap-slider/bootstrap-slider.js"));

            bundles.Add(new ScriptBundle("~/bundles/flot").Include(
                    "~/Scripts/plugins/flot/jquery.flot.min.js",
                    "~/Scripts/plugins/flot/jquery.flot.resize.min.js",
                    "~/Scripts/plugins/flot/jquery.flot.pie.min.js",
                    "~/Scripts/plugins/flot/jquery.flot.categories.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/iCheck").Include(
                    "~/Scripts/plugins/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/input-mask").Include(
                    "~/Scripts/plugins/input-mask/jquery.inputmask.js",
                    "~/Scripts/plugins/input-mask/jquery.inputmask.date.extensions.js",
                    "~/Scripts/plugins/input-mask/jquery.inputmask.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/ionslider").Include(
                    "~/Scripts/plugins/ionslider/ion.rangeSlider.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryKnob").Include(
                    "~/Scripts/plugins/jqueryKnob/jquery.knob.js"));

            bundles.Add(new ScriptBundle("~/bundles/jvectormap").Include(
                    "~/Scripts/plugins/jvectormap/jquery-jvectormap-1.2.2.min.js",
                    "~/Scripts/plugins/jvectormap/jquery-jvectormap-world-mill-en.js"));

            bundles.Add(new ScriptBundle("~/bundles/misc").Include(
                    "~/Scripts/plugins/misc/html5shiv.js",
                    "~/Scripts/plugins/misc/jquery.ba-resize.min.js",
                    "~/Scripts/plugins/misc/jquery.placeholder.js",
                    "~/Scripts/plugins/misc/modernizr.min.js",
                    "~/Scripts/plugins/misc/respond.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/morris").Include(
                    "~/Scripts/plugins/morris/morris.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/slimScroll").Include(
                    "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));


            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                    "~/Scripts/AdminLTE/app.js",
                    "~/Scripts/AdminLTE/demo.js"));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE_Dashboard").Include(
               "~/Scripts/AdminLTE/dashboard.js"));

            bundles.Add(new ScriptBundle("~/bundles/Calendar").Include(
               "~/Scripts/plugins/calendar/moment.min.js",
               "~/Scripts/plugins/calendar/fullcalendar.min.js"));


            bundles.Add(new StyleBundle("~/Content/css/AdminLTE").Include(
                      "~/Content/css/AdminLTE.css",
                      "~/Content/css/skins/_all-skins.css",
                      "~/Content/css/skins/skin-black.css",
                      "~/Content/css/iCheck/square/blue.css",
                      "~/Content/css/iCheck/all.css",
                      "~/Content/css/knockout-file-bindings.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap").Include(
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/select2.min.css"));

            bundles.Add(new StyleBundle("~/Content/css/bootstrap-slider").Include(
                    "~/Content/css/bootstrap-slider/slider.css"));

            bundles.Add(new StyleBundle("~/Content/css/datatables").Include(
                      "~/Content/css/datatables/dataTables.bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/css/iCheck").Include(
                   "~/Content/css/iCheck/all.css"));

            bundles.Add(new StyleBundle("~/Content/css/ionslider").Include(
                   "~/Content/css/ionslider/ion.rangeSlider.css",
                   "~/Content/css/ionslider/ion.rangeSlider.skinNice.css"));

            bundles.Add(new StyleBundle("~/Content/css/jvectormap").Include(
                      "~/Content/css/jvectormap/jquery-jvectormap-1.2.2.css"));

            bundles.Add(new StyleBundle("~/Content/css/morris").Include(
                   "~/Content/css/morris/morris.css"));

        }
    }
}
