namespace EA.Iws.Web.Areas.NotificationApplication.Controllers
{
    using Core.Notification;
    using Prsd.Core.Mediator;
    using Prsd.Core.Web.ApiClient;
    using Prsd.Core.Web.Mvc.Extensions;
    using Requests.Notification;
    using Requests.SharedUsers;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Infrastructure;
    using ViewModels.ReviewUserAccess;

    [Authorize]
    [NotificationOwnerFilter]
    public class ReviewUserAccessController : Controller
    {
      private readonly IMediator mediator;
       public ReviewUserAccessController(IMediator mediator)
        {
          this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> UserList(Guid id)
        {
            var notificationnumber = await mediator.SendAsync(new GetNotificationNumber(id));

            var model = new UserListViewModel();

            var response = await mediator.SendAsync(new GetSharedUsersByNotificationId(id));
            model.NotificationId = id;
            model.NotificationNumber = notificationnumber;
            model.SharedUsers = new List<NotificationSharedUser>(response);
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Remove(Guid id, Guid sharedId)
        {
            var sharedUser = await mediator.SendAsync(new GetSharedUserById(id, sharedId));

            var model = new RemoveUserViewModel
            {
                SharedUserId = sharedId,
                NotificationId = id,
                UserId = sharedUser.UserId,
                EmailId = sharedUser.Email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Remove(RemoveUserViewModel model)
        {
            try
            {
                await mediator.SendAsync(new DeleteSharedUserForNotification(model.NotificationId, model.SharedUserId));

                return RedirectToAction("UserList", "ReviewUserAccess", new { id = model.NotificationId });
            }
            catch (ApiBadRequestException ex)
            {
                this.HandleBadRequest(ex);
                if (ModelState.IsValid)
                {
                    throw;
                }
            }
            return View(model);
        }
    }
}