namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Infrastructure;
    using Requests.Admin;
    using ViewModels;

    public class AssessmentController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public AssessmentController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public ActionResult DateInput()
        {
            return View();
        }

        [HttpPost]
        public async Task<ViewResult> DateInput(DateInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //var setDates = new SetDates { DecisionDate = new DateTime(Convert.ToInt32(model.DecisionYear), Convert.ToInt32(model.DecisionMonth), Convert.ToInt32(model.DecisionDay)) };

            var setDates = new SetDates();
            setDates.DecisionDate = GetDateFromUserInput(model.DecisionDay, model.DecisionMonth, model.DecisionYear);

            using (var client = apiClient())
            {
                var result = await client.SendAsync(User.GetAccessToken(), setDates);
            }
            return View();
        }

        private DateTime? GetDateFromUserInput(int? day, int? month, int? year)
        {
            if (day.HasValue && month.HasValue && year.HasValue)
            {
                return new DateTime(year.Value, month.Value, day.Value);
            }
            return null;
        }
    }
}