namespace EA.Iws.Web.Areas.Movement
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class MovementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Movement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute(
                name: "Movement_default",
                url: "Movement/{notificationId}/{controller}/{action}/{movementId}/{entityId}",
                defaults: new { action = "Index", controller = "Home", movementId = UrlParameter.Optional, entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}