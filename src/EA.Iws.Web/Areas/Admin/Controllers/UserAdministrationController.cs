namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Core.Admin;
    using Infrastructure;
    using Requests.Admin.UserAdministration;
    using ViewModels.UserAdministration;

    [Authorize(Roles = "internal")]
    public class UserAdministrationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;

        public UserAdministrationController(Func<IIwsClient> apiClient)
        {
            this.apiClient = apiClient;
        }

        [HttpGet]
        public async Task<ActionResult> ManageNewUsers()
        {
            using (var client = apiClient())
            {
                var users = await client.SendAsync(User.GetAccessToken(), new GetNewInternalUsers());

                return View(new NewUsersListViewModel
                {
                    Users = users.Select(u => new UserApprovalViewModel(u)).ToArray()
                });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ManageNewUsers(NewUsersListViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var client = apiClient())
            {
                var message = new SetUserApprovals(
                    model.Users
                        .Where(u => u.Action.HasValue)
                        .Select(u => new KeyValuePair<Guid, ApprovalAction>(u.UserData.Id, u.Action.Value))
                        .ToList());

                await client.SendAsync(User.GetAccessToken(), message);

                return RedirectToAction("Index", "Home");
            }
        }
    }
}