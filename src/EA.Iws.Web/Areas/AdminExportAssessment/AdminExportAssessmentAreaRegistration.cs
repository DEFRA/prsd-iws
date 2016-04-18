namespace EA.Iws.Web.Areas.AdminExportAssessment
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminExportAssessmentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "AdminExportAssessment"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                "AdminExportAssessment_default",
                "Admin-Export-Assessment/{id}/{controller}/{action}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new[] { typeof(HomeController).Namespace });
        }
    }
}