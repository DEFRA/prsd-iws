namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Authorization.Permissions;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;
    using Requests.Notification;
    using Requests.Users;
    using ViewModels.ExternalUserAdministration;

    [AuthorizeActivity(UserAdministrationPermissions.CanManageExternalUsers)]
    public class ExternalUserAdministrationController : Controller
    {
        private readonly IMediator mediator;
        private readonly IIwsClient iwsClient;

        public ExternalUserAdministrationController(IMediator mediator, IIwsClient iwsClient)
        {
            this.mediator = mediator;
            this.iwsClient = iwsClient;
        }

        [HttpGet]
        public ActionResult Deactivate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Deactivate(DeactivateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool userExists = await mediator.SendAsync(new ExternalUserExists(model.Email));

            if (!userExists)
            {
                ModelState.AddModelError("Email", DeactivateUserViewModelResources.EmailNotRegistered);
                return View(model);
            }

            var userId = await mediator.SendAsync(new GetUserId(model.Email));

            return RedirectToAction("DeactivateSummary", new { userId });
        }

        [HttpGet]
        public async Task<ActionResult> DeactivateSummary(Guid userId)
        {
            ViewBag.UserId = userId;
            var summary = await mediator.SendAsync(new GetUserNotificationsSummary(userId));
            return View(summary);
        }

        [HttpGet]
        public async Task<ActionResult> DeactivateConfirmation(Guid userId)
        {
            var user = await mediator.SendAsync(new GetUserById(userId.ToString()));
            ViewBag.UserId = userId;
            ViewBag.Email = user.Email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeactivateConfirmation")]
        public async Task<ActionResult> DeactivateConfirmationPost(Guid userId, string email)
        {
            var result = await iwsClient.Registration.DeactivateUser(User.GetAccessToken(), userId.ToString());
            if (result)
            {
                return RedirectToAction("DeactivateSuccess", new { email });
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [HttpGet]
        public ActionResult DeactivateSuccess(string email)
        {
            ViewBag.Email = email;
            return View();
        }
    }
}