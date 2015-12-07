namespace EA.Iws.Web.Areas.ImportMovement
{
    using System.Web.Mvc;
    using Infrastructure;

    public class ImportMovementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ImportMovement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                name: "ImportMovement_default",
                url: "Import-Movement/{Id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(?).Namespace });
        }
    }
}