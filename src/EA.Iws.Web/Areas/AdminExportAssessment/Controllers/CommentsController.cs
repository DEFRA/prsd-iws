namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using Infrastructure;
    using Infrastructure.Authorization;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;
    using ViewModels.Comments;

    [AuthorizeActivity(ExportNotificationPermissions.CanEditInternalComments)]
    public class CommentsController : Controller
    {
        private readonly IMediator mediator;

        public CommentsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(Guid id, NotificationShipmentsCommentsType type = NotificationShipmentsCommentsType.Notification)
        {
            CommentsViewModel model = new CommentsViewModel();
            model.NotificationId = id;
            model.Type = type;

            var comments = await this.mediator.SendAsync(new GetNotificationComments(id));

            if (type == NotificationShipmentsCommentsType.Notification)
            {
                model.Comments = comments.NotificationComments.Where(p => p.ShipmentNumber == 0).ToList();
            }
            else
            {
                model.Comments = comments.NotificationComments.Where(p => p.ShipmentNumber != 0).ToList();
            }
            
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

            var request = new AddNotificationComment(model.NotificationId, User.GetUserId(), model.Comment, model.ShipmentNumber.GetValueOrDefault(), DateTime.Now);

            await this.mediator.SendAsync(request);

            return RedirectToAction("Index", new { id = model.NotificationId });
        }
    }
}