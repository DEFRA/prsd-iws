namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Infrastructure;
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
                    .Select(u => new KeyValuePair<Guid, ApprovalAction>(u.UserData.Id, u.Action.Value))
                    .ToList());

            await mediator.SendAsync(message);

            return RedirectToAction("Index", "Home");
        }
    }
}