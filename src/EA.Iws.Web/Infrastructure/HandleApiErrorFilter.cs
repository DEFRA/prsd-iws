namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Web.ApiClient;

    public class HandleApiErrorFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var apiException = GetApiException(filterContext);
            if (!filterContext.ExceptionHandled && apiException != null)
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = (int)apiException.StatusCode;

                filterContext.ExceptionHandled = true;
            }
        }

        private static ApiException GetApiException(ExceptionContext filterContext)
        {
            var apiException = filterContext.Exception as ApiException;
            if (apiException == null)
            {
                var aggregateException = filterContext.Exception as AggregateException;
                if (aggregateException != null)
                {
                    apiException = aggregateException.InnerException as ApiException;
                }
            }
            return apiException;
        }
    }
}