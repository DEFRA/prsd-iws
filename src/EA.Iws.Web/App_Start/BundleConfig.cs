﻿namespace EA.Iws.Web
{
    using System.Web.Optimization;

    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/vendor/jquery-1.11.0.min.js",
                "~/Scripts/jquery.unobtrusive-ajax.js",
                "~/Scripts/jquery.prevent-double-click.js",
                "~/Scripts/select2/select2.js",
                "~/Scripts/date-input/dateinput.js",
                "~/Scripts/datable.min.js",
                "~/Scripts/jquery.stickem.js"));

            // JQuery 3.x.x is used for all other browsers.
            bundles.Add(new ScriptBundle("~/bundles/jquery-3").Include(
                    "~/Scripts/jquery-3.6.0.js",
                    "~/Scripts/jquery-3.6.0.min.js",
                    "~/Scripts/jquery.unobtrusive-ajax.js",
                    "~/Scripts/jquery-ui-1.13.2.js",
                    "~/Scripts/jquery-ui-1.13.2.min.js",
                    "~/Scripts/jquery.validate.js",
                    "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery-ui-autocomplete-html-extension.js"));

            bundles.Add(new ScriptBundle("~/bundles/helpers").Include(
                "~/Scripts/helpers.js",
                "~/Scripts/address-book.js",
                "~/Scripts/iws-tracking.js",
                "~/Scripts/decision-dropdown.js",
                "~/Scripts/internal-facility-table.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*",
                "~/Scripts/custom-validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk").Include(
                "~/Scripts/govuk/govuk-template.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk-frontend").Include(
                    "~/Scripts/govuk-frontend/govuk-frontend-4.0.1.min.js",
                    "~/Scripts/setup-govuk-frontend.js",
                    "~/govuk/vendor/polyfills/Function/prototype/bind.js",
                    "~/Scripts/show-hide-content.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk_iws").Include(
                "~/Scripts/vendor/modernizr.custom.77028.js",
                "~/Scripts/vendor/details.polyfill.js",
                "~/Scripts/application.js",
                "~/Scripts/business-type-radio-buttons.js",
                "~/Scripts/customs-office-required-radio-buttons.js",
                "~/Scripts/report-input-parameters.js"));

            bundles.Add(new StyleBundle("~/Content/iws-page-ie6").Include(
                "~/Content/iws-page-ie6.css"));

            bundles.Add(new StyleBundle("~/Content/iws-page-ie7").Include(
                "~/Content/iws-page-ie7.css"));

            bundles.Add(new StyleBundle("~/Content/iws-page-ie8").Include(
                "~/Content/iws-page-ie8.css"));

            bundles.Add(new StyleBundle("~/Content/iws-page").Include(
                "~/Content/iws-page.css"));

            bundles.Add(new StyleBundle("~/Content/css/font-awesome")
                .Include("~/Content/css/font-awesome.css"));

            bundles.Add(new StyleBundle("~/Content/select2/css")
                .Include("~/Content/select2/select2.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}