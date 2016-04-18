namespace EA.Iws.Web.Areas.NotificationApplication
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class NotificationApplicationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NotificationApplication";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute(
                name: "NotificationApplication_default",
                url: "Notification-Application/{id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}