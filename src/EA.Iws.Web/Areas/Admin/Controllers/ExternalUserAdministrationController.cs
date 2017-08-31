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
        public async Task<ActionResult> Deactivate(ExternalUserAdministrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool userExists = await mediator.SendAsync(new ExternalUserExists(model.Email));

            if (!userExists)
            {
                ModelState.AddModelError("Email", ExternalUserAdministrationViewModelResources.EmailNotRegistered);
                return View(model);
            }

            var userId = await mediator.SendAsync(new GetUserId(model.Email));

            var userData = await mediator.SendAsync(new GetExternalUserByUserId(userId));

            if (userData.Status == ExternalUserStatus.Inactive)
            {
                ModelState.AddModelError("Email", ExternalUserAdministrationViewModelResources.UserAlreadyDeactivated);
                return View(model);
            }

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

        [HttpGet]
        public ActionResult Reactivate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Reactivate(ExternalUserAdministrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool userExists = await mediator.SendAsync(new ExternalUserExists(model.Email));

            if (!userExists)
            {
                ModelState.AddModelError("Email", ExternalUserAdministrationViewModelResources.EmailNotRegistered);
                return View(model);
            }

            var userId = await mediator.SendAsync(new GetUserId(model.Email));

            var userData = await mediator.SendAsync(new GetExternalUserByUserId(userId));

            if (userData.Status == ExternalUserStatus.Active)
            {
                ModelState.AddModelError("Email", ExternalUserAdministrationViewModelResources.UserAlreadyActive);
                return View(model);
            }

            return RedirectToAction("ReactivateConfirmation", new { userId });
        }

        [HttpGet]
        public async Task<ActionResult> ReactivateConfirmation(Guid userId)
        {
            var user = await mediator.SendAsync(new GetUserById(userId.ToString()));
            ViewBag.UserId = userId;
            ViewBag.Email = user.Email;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("ReactivateConfirmation")]
        public async Task<ActionResult> ReactivateConfirmationPost(Guid userId, string email)
        {
            await mediator.SendAsync(new SetExternalUserStatus(userId.ToString(), ExternalUserStatus.Active));

            return RedirectToAction("ReactivateSuccess", new { email });
        }

        [HttpGet]
        public ActionResult ReactivateSuccess(string email)
        {
            ViewBag.Email = email;
            return View();
        }
    }
}