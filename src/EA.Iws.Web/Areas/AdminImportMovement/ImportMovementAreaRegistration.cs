namespace EA.Iws.Web.Areas.AdminImportMovement
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminImportMovementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminImportMovement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                name: "AdminImportMovement_default",
                url: "Admin-Import-Movement/{Id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(DatesController).Namespace });
        }
    }
}