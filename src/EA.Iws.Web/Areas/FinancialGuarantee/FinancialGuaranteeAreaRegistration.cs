namespace EA.Iws.Web.Areas.FinancialGuarantee
{
    using System.Web.Mvc;
    using Controllers;
    using Infrastructure;

    public class FinancialGuaranteeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FinancialGuarantee";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLowercaseDashedRoute(
                "FinancialGuarantee_default",
                "Financial-Guarantee/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace });
        }
    }
}