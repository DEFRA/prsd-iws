namespace EA.Iws.Web.Areas.AdminExportMovement
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminExportMovementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "AdminExportMovement"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                "AdminExportMovement_default",
                "Admin-Export-Movement/{id}/{controller}/{action}/{entityId}",
                new { action = "Index", entityId = UrlParameter.Optional },
                new[] { typeof(InternalCaptureController).Namespace });
        }
    }
}