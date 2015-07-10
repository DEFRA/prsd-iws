namespace EA.Iws.Web.Areas.FinancialGuarantee
{
    using System.Web.Mvc;

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
            context.MapRoute(
                "FinancialGuarantee_default",
                "FinancialGuarantee/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional });
        }
    }
}