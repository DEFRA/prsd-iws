namespace EA.Iws.Web.Areas.NotificationApplication
{
    using System.Web.Mvc;

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
                "NotificationApplication_default",
                "NotificationApplication/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}