namespace EA.Iws.Web.Areas.ImportNotification
{
    using Infrastructure;
    using System.Web.Mvc;
    using Controllers;

    public class ImportNotificationAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "ImportNotification"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                name: "ImportNotification_default",
                url: "Import-Notification/{id}/{controller}/{action}/{entityId}",
                defaults: new { action = "Index", controller = "Home", entityId = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}