namespace EA.Iws.Web.Areas.AdminMovement
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminMovementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "AdminMovement"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                "AdminMovement_default",
                "Admin-Movement/{id}/{controller}/{action}/{entityId}",
                new { action = "Index", entityId = UrlParameter.Optional },
                new[] { typeof(InternalCaptureController).Namespace });
        }
    }
}