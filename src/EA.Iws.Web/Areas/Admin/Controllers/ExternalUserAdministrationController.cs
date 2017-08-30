namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
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

        public ExternalUserAdministrationController(IMediator mediator)
        {
            this.mediator = mediator;
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
            await mediator.SendAsync(new SetExternalUserStatus(userId.ToString(), ExternalUserStatus.Inactive));

            return RedirectToAction("DeactivateSuccess", new { email });
        }

        [HttpGet]
        public ActionResult DeactivateSuccess(string email)
        {
            ViewBag.Email = email;
            return View();
        }
    }
}