namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;
    using Requests.Notification;
    using ViewModels.ChangeNotificationOwner;

    [Authorize(Roles = "internal")]
    public class ChangeNotificationOwnerController : Controller
    {
        private readonly IMediator mediator;
        private const string UserIdKey = "UserIdKey";
        private const string NotificationNumberKey = "NotificationNumber";
        private const string NewUserEmailKey = "NewUserEmail";

        public ChangeNotificationOwnerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id)
        {
            var data = await mediator.SendAsync(new GetUserByExportNotificationId(id));
            var model = new ChangeNotificationOwnerViewModel(data);
            model.NotificationId = id;

            var allUsers = await mediator.SendAsync(new GetAllUsers());

            model.AllUsers = allUsers.Where(u => u.UserId != data.UserId).ToList();
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, ChangeNotificationOwnerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var allUsers = await mediator.SendAsync(new GetAllUsers());

                model.AllUsers = allUsers.ToList();

                return View(model);
            }

            TempData[UserIdKey] = model.SelectedUser;

            return RedirectToAction("Confirm", new { id = model.NotificationId });
        }

        [HttpGet]
        public async Task<ActionResult> Confirm(Guid id)
        {
            object userId;
            if (TempData.TryGetValue(UserIdKey, out userId))
            {
                var notificationNumber = (await mediator.SendAsync(new GetNotificationBasicInfo(id))).NotificationNumber;
                var newUser = await mediator.SendAsync(new GetUserById(userId.ToString()));

                return View(new ConfirmViewModel
                {
                    NotificationId = id,
                    NotificationNumber = notificationNumber,
                    NewUser = newUser
                });
            }

            return RedirectToAction("Index", new {id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(Guid id, ConfirmViewModel model)
        {
            await mediator.SendAsync(new ChangeUser(model.NotificationId, new Guid(model.NewUser.UserId)));

            TempData[NotificationNumberKey] = model.NotificationNumber;
            TempData[NewUserEmailKey] = model.NewUser.Email;
                        
            return RedirectToAction("Success", new { id });
        }

        [HttpGet]
        public ActionResult Success(Guid id)
        {
            object newUserEmail;
            object notificationNumber;

            if (TempData.TryGetValue(NotificationNumberKey, out notificationNumber) &&
                TempData.TryGetValue(NewUserEmailKey, out newUserEmail))
            {
                var model = new SuccessViewModel
                {
                    NotificationNumber = notificationNumber.ToString(),
                    NewUserEmail = newUserEmail.ToString()
                };

                return View(model);
            }

            return View();
        }
    }
}