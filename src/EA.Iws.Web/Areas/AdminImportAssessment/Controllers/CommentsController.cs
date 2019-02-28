namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationAssessment;
    using ViewModels.Comments;

    [AuthorizeActivity(ImportNotificationPermissions.CanEditComments)]
    public class CommentsController : Controller
    {
        private readonly IMediator mediator;

        public CommentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Index(Guid id)
        {
            CommentsViewModel model = new CommentsViewModel();
            model.NotificationId = id;
            return View(model);
        }

        [HttpGet]
        public ActionResult Add(Guid id)
        {
            AddCommentsViewModel model = new AddCommentsViewModel();
            model.NotificationId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(AddCommentsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ModelIsValid = false;
                return View(model);
            }

            Guid userId = Guid.Parse(User.GetUserId());

            var request = new AddImportNotificationComment(model.NotificationId, userId, model.Comment, model.ShipmentNumber.GetValueOrDefault(), DateTime.Now);

            await this.mediator.SendAsync(request);

            return RedirectToAction("Index", new { id = model.NotificationId });
        }
    }
}