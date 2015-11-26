namespace EA.Iws.Web.Areas.AdminNotificationMovements
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminNotificationMovementsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminNotificationMovements";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(name: "AdminNotificationMovements_default",
                url: "Admin-Notification-Movements/{id}/{controller}/{action}",
                defaults: new { action = "Index", controller = "Home" },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}