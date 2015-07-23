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
    using Services;
    using ViewModels.UserAdministration;

    [Authorize(Roles = "internal")]
    public class UserAdministrationController : Controller
    {
        private readonly Func<IIwsClient> apiClient;
        private readonly IEmailService emailService;
        private readonly IEmailTemplateService emailTemplateService;

        public UserAdministrationController(Func<IIwsClient> apiClient,
            IEmailService emailService,
            IEmailTemplateService emailTemplateService)
        {
            this.apiClient = apiClient;
            this.emailService = emailService;
            this.emailTemplateService = emailTemplateService;
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
                        .Select(u => new KeyValuePair<string, ApprovalAction>(u.User.Id, u.Action.Value))
                        .ToList());

                var updateSuccessful = await client.SendAsync(User.GetAccessToken(), message);

                if (updateSuccessful)
                {
                    await EmailUserUpdates(model);
                }

                return RedirectToAction("Index", "Home");
            }
        }

        private async Task EmailUserUpdates(NewUsersListViewModel model)
        {
            EmailTemplate approvedTemplate = null;
            EmailTemplate rejectedTemplate = null;

            if (model.Users.Any(u => u.Action == ApprovalAction.Approve))
            {
                approvedTemplate = emailTemplateService.TemplateWithDynamicModel("InternalRegistrationApproved",
                new
                {
                    SignInLink = Url.Action("Login", "Account", new { area = string.Empty }, Request.Url.Scheme)
                });
            }

            if (model.Users.Any(u => u.Action == ApprovalAction.Reject))
            {
                rejectedTemplate = emailTemplateService.TemplateWithDynamicModel("InternalRegistrationRejected",
                null);
            }

            foreach (var user in model.Users)
            {
                if (user.Action == ApprovalAction.Approve)
                {
                    var email = emailService.GenerateMailMessageWithHtmlAndPlainTextParts(emailService.MailFrom, user.User.Email,
                        "IWS Registration Approved", approvedTemplate);

                    await emailService.SendAsync(email);
                }
                else if (user.Action == ApprovalAction.Reject)
                {
                    var email = emailService.GenerateMailMessageWithHtmlAndPlainTextParts(emailService.MailFrom, user.User.Email,
                        "IWS Registration Rejected", rejectedTemplate);

                    await emailService.SendAsync(email);
                }
            }
        }
    }
}