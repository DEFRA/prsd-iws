namespace EA.Iws.Web.Areas.NotificationApplication
{
    using System.Web.Mvc;
    using Controllers;

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
            context.MapRoute(
                name: "NotificationApplication_default",
                url: "NotificationApplication/{id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}