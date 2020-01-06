namespace EA.Iws.Api
{
    using System.Collections.Generic;
    using System.Web.Http.Filters;
    using Elmah.Contrib.WebApi;
    using Filters;
    using Services;

    public class FilterConfig
    {
        private IList<IFilter> collection;

        public IList<IFilter> Collection
        {
            get { return collection; }
            set { collection = value; }
        }

        public FilterConfig(IAppConfiguration configuration)
        {
            collection = new List<IFilter> { new ElmahHandleErrorApiAttribute() };

            if (configuration.MaintenanceMode)
            {
                Collection.Add(new MaintenanceModeFilter());
            }
        }
    }
}