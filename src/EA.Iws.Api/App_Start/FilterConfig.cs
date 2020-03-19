namespace EA.Iws.Api
{
    using System.Collections.Generic;
    using System.Web.Http.Filters;
    using Elmah.Contrib.WebApi;
    using Filters;
    using Services;

    public class FilterConfig
    {
        public IList<IFilter> Collection { get; set; }

        public FilterConfig(IAppConfiguration configuration)
        {
            Collection = new List<IFilter> { new ElmahHandleErrorApiAttribute() };
            
            if (configuration.MaintenanceMode)
            {
                Collection.Add(new MaintenanceModeFilter());
            }
        }
    }
}