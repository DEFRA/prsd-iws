namespace EA.Iws.Web.Areas.AdminImportNotificationMovements
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminImportNotificationMovementsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminImportNotificationMovements";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute("AdminImportNotificationMovements_default",
                "Admin-Import-Notification-Movements/{id}/{controller}/{action}",
                new { action = "Index", controller = "Home" },
                new[] { typeof(CaptureController).Namespace });
        }
    }
}