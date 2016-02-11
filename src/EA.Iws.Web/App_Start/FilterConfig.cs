namespace EA.Iws.Web
{
    using System.Web.Mvc;
    using Infrastructure;
    using Infrastructure.VirusScanning;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Order of filters depends on their type - http://stackoverflow.com/a/10232898

            // Authorization filters run before actions, so lower order will run first
            filters.Add(new RequireHttpsAttribute(), order: 1);
            filters.Add(new EmailVerificationRequiredAttribute(), order: 2);
            filters.Add(new OrganisationRequiredAttribute(), order: 3);
            filters.Add(new AdminApprovalRequiredAttribute(), order: 4);

            // Error filters run after actions, so higher order will run first
            filters.Add(new HandleApiErrorFilter(), order: 5);
            filters.Add(new AntiForgeryErrorFilter(), order: 6);
            filters.Add(new VirusFoundFilter(), order: 7);
        }
    }
}