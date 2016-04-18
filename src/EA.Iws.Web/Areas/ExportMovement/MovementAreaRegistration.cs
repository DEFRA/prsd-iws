namespace EA.Iws.Web.Areas.ExportMovement
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class ExportMovementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ExportMovement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute(
                name: "ExportMovement_default",
                url: "Export-Movement/{Id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}