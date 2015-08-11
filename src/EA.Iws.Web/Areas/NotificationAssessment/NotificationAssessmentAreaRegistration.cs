namespace EA.Iws.Web.Areas.NotificationAssessment
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class NotificationAssessmentAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get { return "NotificationAssessment"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapLowercaseDashedRoute(
                "NotificationAssessment_default",
                "Notification-Assessment/{id}/{controller}/{action}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new[] { typeof(HomeController).Namespace });
        }
    }
}