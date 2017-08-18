namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Users;
    using ViewModels.ChangeNotificationOwner;

    [AuthorizeActivity(typeof(ChangeUser))]
    public class ChangeNotificationOwnerController : Controller
    {
        private readonly IMediator mediator;

        public ChangeNotificationOwnerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new ChangeOwnerViewModel(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ChangeOwnerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userExists = await mediator.SendAsync(new ExternalUserExists(model.EmailAddress));

            if (!userExists)
            {
                ModelState.AddModelError("EmailAddress",
                    "The email address you have entered is not registered in IWS Online");
                return View(model);
            }

            TempData["EmailAddress"] = model.EmailAddress;

            return RedirectToAction("Confirm", new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id)
        {
            var emailAddress = TempData["EmailAddress"];

            if (emailAddress == null)
            {
                return RedirectToAction("Index");
            }

            var notificationNumber = await mediator.SendAsync(new GetNotificationNumber(id));

            var model = new ConfirmViewModel
            {
                EmailAddress = (string)emailAddress,
                NotificationNumber = notificationNumber,
                NotificationId = id
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ConfirmViewModel model)
        {
            var userId = await mediator.SendAsync(new GetUserId(model.EmailAddress));
            await mediator.SendAsync(new ChangeUser(model.NotificationId, userId));

            TempData["ConfirmViewModel"] = model;

            return RedirectToAction("Success", new { id = model.NotificationId });
        }

        [HttpGet]
        public ActionResult Success(Guid id)
        {
            var model = (ConfirmViewModel)TempData["ConfirmViewModel"];

            return View(model);
        }
    }
}