namespace EA.Iws.Web.Areas.Reports
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class ReportsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Reports"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                "Reports_default",
                "Reports/{controller}/{action}",
                new { controller = "Home", action = "Index" }, new[] { typeof(ExportNotificationsController).Namespace });
        }
    }
}