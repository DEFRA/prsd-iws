namespace EA.Iws.Web.Areas.MovementDocument
{
    using Controllers;
    using System.Web.Mvc;
    using Infrastructure;

    public class MovementDocumentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MovementDocument";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute(
                name: "MovementDocument_default",
                url: "Movement-Document/{id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}