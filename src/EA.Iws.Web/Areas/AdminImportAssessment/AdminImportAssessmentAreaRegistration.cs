namespace EA.Iws.Web.Areas.AdminImportAssessment
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class AdminImportAssessmentAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AdminImportAssessment";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute(
                "AdminImportAssessment_default",
                "Admin-Import-Assessment/{id}/{controller}/{action}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new[] { typeof(KeyDatesController).Namespace });
        }
    }
}