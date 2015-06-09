namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Requests.Shared;

    public static class ControllerExtensions
    {
        public static async Task BindCountryList(this Controller controller, Func<IIwsClient> apiClient, bool setDefaultAsUnitedKingdom = true)
        {
            using (var client = apiClient())
            {
                await controller.BindCountryList(client, setDefaultAsUnitedKingdom);
            }
        }

        public static async Task BindCountryList(this Controller controller, IIwsClient client, bool setDefaultAsUnitedKingdom = true)
        {
            var response = await client.SendAsync(new GetCountries());

            var defaultId = response.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id;

            if (setDefaultAsUnitedKingdom)
            {
                controller.ViewBag.Countries = new SelectList(response, "Id", "Name", defaultId);
            }
            else
            {
                controller.ViewBag.Countries = new SelectList(response, "Id", "Name");
            }
        }

        public static Guid GetDefaultCountryId(this Controller controller)
        {
            var countries = (SelectList)controller.ViewBag.Countries;

            var defaultId = countries.Single(c => c.Text.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Value;

            return new Guid(defaultId);
        }
    }
}