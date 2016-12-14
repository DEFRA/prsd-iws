namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin.UserAdministration;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.Admin.UserAdministration;
    using ViewModels.UserAdministration;

    [AuthorizeActivity(typeof(SetUserApprovals))]
    public class UserAdministrationController : Controller
    {
        private readonly IMediator mediator;

        public UserAdministrationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> ManageNewUsers()
        {
            var users = await mediator.SendAsync(new GetNewInternalUsers());

            return View(new NewUsersListViewModel
            {
                Users = users.Select(u => new UserApprovalViewModel(u)).ToArray()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageNewUsers(NewUsersListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var message = new SetUserApprovals(
                model.Users
                    .Where(u => u.Action.HasValue)
                    .Select(u => new UserApproval(u.UserData.Id, u.Action.Value, u.AssignedRole))
                    .ToList());

            await mediator.SendAsync(message);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> ManageExistingUsers()
        {
            var users = await mediator.SendAsync(new GetExistingInternalUsers());

            var model = new ExistingUsersListViewModel
            {
                Users = users.Select(u => new ManageUserViewModel(u)).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageExistingUsers(ManageUserViewModel model)
        {
            await mediator.SendAsync(new UpdateInternalUser(model.UserId, model.AssignedStatus, model.AssignedRole));

            return RedirectToAction("ManageExistingUsers");
        }
    }
}