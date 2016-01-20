namespace EA.Iws.Web
{
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.VirusScanning;
    using Prsd.Core.Web.Mvc.Filters;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RequireHttpsAttribute(), order: 1);
            filters.Add(new AntiForgeryErrorFilter(), order: 2);
            filters.Add(new VirusFoundFilter(), order: 3);
            filters.Add(new RequestAuthorizationErrorFilter(), order: 4);
            filters.Add(new HandleApiErrorAttribute(), order: 5);
            filters.Add(new EmailVerificationRequiredAttribute(), order: 6);
            filters.Add(new OrganisationRequiredAttribute(), order: 7);
            filters.Add(new AdminApprovalRequired(), order: 8);
            filters.Add(new HandleErrorAttribute() { View = "~/Views/Errors/InternalError.cshtml" }, order: 9);
        }
    }
}