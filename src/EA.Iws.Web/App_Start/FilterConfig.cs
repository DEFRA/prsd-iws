namespace EA.Iws.Web
{
    using System.Web.Mvc;
    using Prsd.Core.Web.Mvc.Filters;
    using Infrastructure;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleApiErrorAttribute());
            filters.Add(new OrganisationRegAuthorizeAttribute());
        }
    }
}