namespace EA.Iws.Web.Areas.AdminExportNotificationMovements
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminExportNotificationMovementsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AdminExportNotificationMovements";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(name: "AdminExportNotificationMovements_default",
                url: "Admin-Export-Notification-Movements/{id}/{controller}/{action}",
                defaults: new { action = "Index", controller = "Home" },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}