namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Web.Mvc;
    using Prsd.Core.Web.ApiClient;

    public class HandleApiErrorFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.IsChildAction)
            {
                return;
            }

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            var apiException = GetApiException(filterContext);

            if (apiException == null)
            {
                return;
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = (int)apiException.StatusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
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