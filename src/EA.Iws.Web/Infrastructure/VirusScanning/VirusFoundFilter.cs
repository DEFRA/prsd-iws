namespace EA.Iws.Web.Infrastructure.VirusScanning
{
    using System.Web.Mvc;

    public class VirusFoundFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is VirusFoundException)
            {
                filterContext.HttpContext.Response.Clear();
                filterContext.HttpContext.Response.StatusCode = 400;

                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Errors/VirusDetected.cshtml"
                };

                filterContext.ExceptionHandled = true;
            }
        }
    }
}