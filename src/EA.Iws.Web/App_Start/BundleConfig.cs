namespace EA.Iws.Web
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
                      "~/Scripts/jquery.prevent-double-click.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                "~/Scripts/jquery-ui-{version}.js",
                "~/Scripts/jquery.select-to-autocomplete.js"));

            bundles.Add(new ScriptBundle("~/bundles/address-book").Include(
                "~/Scripts/address-book.js",
                "~/Scripts/jquery-ui-autocomplete-html-extension.js"));

            bundles.Add(new ScriptBundle("~/bundles/helpers").Include(
                "~/Scripts/helpers.js"));

            bundles.Add(new ScriptBundle("~/bundles/decision-dropdown").Include(
                "~/Scripts/decision-dropdown.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/custom-validation.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk_toolkit").Include(
                      "~/Scripts/govuk_toolkit/vendor/polyfills/bind.js",
                      "~/Scripts/govuk_toolkit/govuk/selection-buttons.js"));

            bundles.Add(new ScriptBundle("~/bundles/govuk_iws").Include(
                      "~/Scripts/vendor/modernizr.custom.77028.js",
                      "~/Scripts/vendor/details.polyfill.js",
                      "~/Scripts/application.js",
                      "~/Scripts/business-type-radio-buttons.js"));

            bundles.Add(new ScriptBundle("~/bundles/facility").Include(
                "~/Scripts/internal-facility-table.js"));

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

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}