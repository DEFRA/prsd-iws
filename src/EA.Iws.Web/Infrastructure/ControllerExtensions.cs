namespace EA.Iws.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Shared;
    using Prsd.Core.Mediator;
    using Requests.Shared;

    public static class ControllerExtensions
    {
        public static async Task BindCountryList(this Controller controller, IIwsClient client, bool setDefaultAsUnitedKingdom = true)
        {
            var response = await client.SendAsync(new GetCountries());

            BindCountriesToViewBag(controller, response, setDefaultAsUnitedKingdom);
        }

        public static async Task BindCountryList(this Controller controller, IMediator mediator,
            bool setDefaultAsUnitedKingdom = true)
        {
            var response = await mediator.SendAsync(new GetCountries());

            BindCountriesToViewBag(controller, response, setDefaultAsUnitedKingdom);
        }

        private static void BindCountriesToViewBag(Controller controller, List<CountryData> response, bool setDefaultAsUnitedKingdom)
        {
            var defaultId = response.Single(c => c.Name.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Id;

            if (setDefaultAsUnitedKingdom)
            {
                controller.ViewBag.Countries = new SelectList(response, "Id", "Name", defaultId);
            }
            else
            {
                response.Insert(0, new CountryData { Id = Guid.Empty, Name = string.Empty });
                controller.ViewBag.Countries = new SelectList(response, "Id", "Name", Guid.Empty);
            }
        }

        public static Guid GetDefaultCountryId(this Controller controller)
        {
            var countries = (SelectList)controller.ViewBag.Countries;

            var defaultId = countries.Single(c => c.Text.Equals("United Kingdom", StringComparison.InvariantCultureIgnoreCase)).Value;

            return new Guid(defaultId);
        }

        public static void RemoveModelStateErrors(this Controller controller)
        {
            foreach (var modelValue in controller.ModelState.Values)
            {
                modelValue.Errors.Clear();
            }
        }
    }
}