namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
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

    [Authorize(Roles = "internal")]
    public class DecisionController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public DecisionController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new DecisionViewModel
            {
                NotificationId = id, 
                DecisionTypes = GetDecisionTypes()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ViewResult> Index(DecisionViewModel model)
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
                DecisionMade = model.DecisionMadeDate.AsDateTime(),
                ConsentedFrom = model.ConsentValidFromDate.AsDateTime(),
                ConsentedTo = model.ConsentValidToDate.AsDateTime(),
                DecisionType = Convert.ToInt32(model.DecisionType)
            };

            using (var client = apiClient())
            {
                await client.SendAsync(User.GetAccessToken(), setDates);
            }
            model.DecisionTypes = GetDecisionTypes();
            return View(model);
        }

        private SelectList GetDecisionTypes()
        {
            return new SelectList(EnumHelper.GetValues(typeof(DecisionType)), "Key", "Value");
        }
    }
}