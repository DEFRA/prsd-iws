namespace EA.Iws.Web.Areas.NotificationMovements
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class NotificationMovementsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "NotificationMovements"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute("NotificationMovements_default",
                "Notification-Movements/{notificationId}/{controller}/{action}",
                new { action = "Index", controller = "Home" }, new[] { typeof(HomeController).Namespace });
        }
    }
}