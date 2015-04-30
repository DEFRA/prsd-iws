namespace EA.Iws.Web.Infrastructure
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;
    using Api.Client;
    using Newtonsoft.Json.Linq;

    public static class ControllerExtensions
    {
        public static void AddValidationErrorsToModelState(this Controller controller, HttpResponseMessage response)
        {
            var errorResponse = response.Content.ReadAsAsync<HttpError>().Result;
            if (errorResponse != null)
            {
                if (!errorResponse.ContainsKey("ModelState"))
                {
                    return;
                }
                var modelStateErrors = errorResponse["ModelState"] as JObject;
                if (modelStateErrors != null)
                {
                    foreach (var error in modelStateErrors)
                    {
                        foreach (var message in error.Value.Values<string>())
                        {
                            controller.ModelState.AddModelError(string.Empty, message);
                        }
                    }
                }
            }
        }

        public static void AddValidationErrorsToModelState(this Controller controller, Response response)
        {
            foreach (var error in response.Errors)
            {
                controller.ModelState.AddModelError(string.Empty, error);
            }
        }
    }
}