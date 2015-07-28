namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Admin;
    using Infrastructure;
    using Prsd.Core.Helpers;
    using Requests.Admin;
    using ViewModels;

    public class DecisionController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public DecisionController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Decision(Guid id)
        {
            var model = new DecisionViewModel();
            model.NotificationId = id;
            model.DecisionTypes = GetDecisionTypes();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Decision(DecisionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DecisionTypes = GetDecisionTypes();
                return View(model);
            }

            var setDates = new SetDecision
            {
                NotificationApplicationId = model.NotificationId,
                ConditionsOfConsent = model.ConditionsOfConsent,
                DecisionMade = GetDateFromUserInput(model.DecisionMadeDay, model.DecisionMadeMonth, model.DecisionMadeYear),
                ConsentedFrom = GetDateFromUserInput(model.ConsentValidFromDay, model.ConsentValidFromMonth, model.ConsentValidFromYear),
                ConsentedTo = GetDateFromUserInput(model.ConsentValidToDay, model.ConsentValidToMonth, model.ConsentValidToYear),
                DecisionType = Convert.ToInt32(model.DecisionType)
            };

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setDates);
            }
            model.DecisionTypes = GetDecisionTypes();
            return View(model);
        }

        private DateTime? GetDateFromUserInput(int? day, int? month, int? year)
        {
            if (day.HasValue && month.HasValue && year.HasValue)
            {
                return new DateTime(year.Value, month.Value, day.Value);
            }
            return null;
        }

        private SelectList GetDecisionTypes()
        {
            return new SelectList(EnumHelper.GetValues(typeof(DecisionType)), "Key", "Value");
        }
    }
}