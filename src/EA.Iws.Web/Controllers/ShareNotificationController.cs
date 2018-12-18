namespace EA.Iws.Web.Controllers
{
    using Core.Notification;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Requests.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using ViewModels.ShareNotification;

    [AuthorizeActivity(typeof(AddSharedUser))]
    public class ShareNotificationController : Controller
    {
        private readonly IMediator mediator;

        public ShareNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var model = new ShareNotificationViewModel(id);
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id)
        {
            List<NotificationSharedUser> sharedUsers = (List<NotificationSharedUser>)TempData["SharedUsers"];
            TempData["SharedUsers"] = sharedUsers;

            SharedUserListConfirmViewModel model = new SharedUserListConfirmViewModel(id, sharedUsers);

            var notification = await mediator.SendAsync(new GetNotificationBasicInfo(model.NotificationId));

            model.ConfirmTitle = model.ConfirmTitle.Replace("{notification}", notification.NotificationNumber);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ShareNotificationViewModel model, string command, string removeId)
        {
            // User is removing a user from the list
            if (removeId != null)
            {
                model.SelectedSharedUsers.RemoveAll(c => c.UserId.ToString() == removeId);
            }

            // Use is finished adding emails and proceeding to next page
            if (command == "continue")
            {
                SharedUserListConfirmViewModel confirmViewModel = new SharedUserListConfirmViewModel(id, model.SelectedSharedUsers);

                TempData["SharedUsers"] = confirmViewModel.SharedUsers;
                return RedirectToAction("Confirm", "ShareNotification", new { id = id });
            }

            // User has tried to add an email address
            if (command == "addshareduser")
            {
                // Check validation of model for correct number of email addresses and email address in correct format
                if (!ModelState.IsValid)
                {
                    model.SetSharedUsers(model.SelectedSharedUsers);
                    return View(model);
                }

                Guid userId;
                // This method doesn't handle nicely so if a user doesn't exist, it throws an exception.
                try
                {
                    userId = await mediator.SendAsync(new GetUserId(model.EmailAddress));
                }
                catch (Exception)
                {
                    ModelState.AddModelError("EmailAddress", "Enter a valid email address");
                    model.SetSharedUsers(model.SelectedSharedUsers);
                    return View(model);
                }

                var isInternalUser = await mediator.SendAsync(new ExternalUserExists(model.EmailAddress));

                if (!isInternalUser)
                {
                    ModelState.AddModelError("EmailAddress", "Email address can't be an internal user");
                    model.SetSharedUsers(model.SelectedSharedUsers);
                    return View(model);
                }

                // If user id is not already in list then add it to the list
                if (userId != null && model.SelectedSharedUsers.Count(p => p.UserId == userId.ToString()) == 0)
                {
                    model.SelectedSharedUsers.Add(new NotificationSharedUser { Email = model.EmailAddress, UserId = userId.ToString(), NotificationId = model.NotificationId });
                }     
            }

            this.PrepareReturnModel(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(Guid id, SharedUserListConfirmViewModel model)
        {
            model.SharedUsers = (List<NotificationSharedUser>)TempData["SharedUsers"];

            return View(model);
        }

        private ShareNotificationViewModel PrepareReturnModel(ShareNotificationViewModel model)
        {
            model.EmailAddress = string.Empty;
            model.SetSharedUsers(model.SelectedSharedUsers);

            ModelState.Clear();
            return model;
        }
    }
}