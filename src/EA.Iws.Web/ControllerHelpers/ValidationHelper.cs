namespace EA.Iws.Web.ControllerHelpers
{
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Mvc;
    using Newtonsoft.Json.Linq;

    public static class ValidationHelper
    {
        public static void AddValidationErrorsToModelState(Controller controller, HttpResponseMessage response)
        {
            var errorResponse = response.Content.ReadAsAsync<HttpError>().Result;
            if (errorResponse != null)
            {
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
    }
}