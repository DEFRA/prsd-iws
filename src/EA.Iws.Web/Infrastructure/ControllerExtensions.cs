namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Requests.Registration;

    public static class ControllerExtensions
    {
        public static async Task BindCountryList(this Controller controller, Func<IIwsClient> apiClient)
        {
            using (var client = apiClient())
            {
                await controller.BindCountryList(client);
            }
        }

        public static async Task BindCountryList(this Controller controller, IIwsClient client)
        {
            var response = await client.SendAsync(new GetCountries());

            controller.ViewBag.Countries = new SelectList(response, "Id", "Name",
                response.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id);
        }
    }
}