namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotificationAssessment;
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
        public async Task<ActionResult> Index(Guid id, string filter, NotificationShipmentsCommentsType type = NotificationShipmentsCommentsType.Notification, int page = 1)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            if (filter == "date" && TempData.ContainsKey("startDate") && TempData.ContainsKey("endDate"))
            {
                startDate = DateTime.Parse(TempData["startDate"].ToString());
                endDate = DateTime.Parse(TempData["endDate"].ToString());

                TempData["startDate"] = filter == "date" ? startDate.ToString() : null;
                TempData["endDate"] = filter == "date" ? endDate.ToString() : null;
            }
            else
            {
                startDate = DateTime.MinValue;
                endDate = DateTime.MaxValue;
                TempData.Remove("startDate");
                TempData.Remove("endDate");
            }

            int? shipmentNumber;

            if (filter == "shipment" && TempData.ContainsKey("shipmentNumber"))
            {
                shipmentNumber = int.Parse(TempData["shipmentNumber"].ToString());

                TempData["shipmentNumber"] = shipmentNumber.ToString();
            }
            else
            {
                shipmentNumber = null;
                TempData.Remove("shipmentNumber");
            }

            var comments = await this.mediator.SendAsync(new GetImportNotificationComments(id, type, page, startDate, endDate, shipmentNumber));

            CommentsViewModel model = new CommentsViewModel
            {
                NotificationId = id,
                Type = type,
                SelectedFilter = filter,
                TotalNumberOfComments = comments.NumberOfComments,
                ShipmentNumber = shipmentNumber,
                PageNumber = comments.PageNumber,
                PageSize = comments.PageSize,
                TotalNumberOfFilteredComments = comments.NumberOfFilteredComments,
                Comments = comments.NotificationComments.ToList()
            };

            model.SetDates(startDate, endDate);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(Guid id, CommentsViewModel model, string command)
        {
            if (command == null)
            {
                return RedirectToAction("Index", new { filter = model.SelectedFilter, type = model.Type });
            }

            if (command == "search" && !ModelState.IsValid)
            {
                var comments = await this.mediator.SendAsync(new GetImportNotificationComments(id, model.Type, model.PageNumber, null, null, null));
                model.TotalNumberOfComments = comments.NumberOfComments;
                model.Comments = comments.NotificationComments.ToList();

                return View(model);
            }

            if (model.SelectedFilter == "date")
            {
                DateTime startDate = new DateTime(model.StartYear.GetValueOrDefault(), model.StartMonth.GetValueOrDefault(), model.StartDay.GetValueOrDefault());
                DateTime endDate = new DateTime(model.EndYear.GetValueOrDefault(), model.EndMonth.GetValueOrDefault(), model.EndDay.GetValueOrDefault());

                TempData["startDate"] = startDate;
                TempData["endDate"] = endDate;
            }

            if (model.SelectedFilter == "shipment")
            {
                TempData["shipmentNumber"] = model.ShipmentNumber;
            }

            return RedirectToAction("Index", new { filter = model.SelectedFilter, type = model.Type });
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

            var request = new AddImportNotificationComment(model.NotificationId, User.GetUserId(), model.Comment, model.ShipmentNumber.GetValueOrDefault(), DateTime.Now);

            await this.mediator.SendAsync(request);

            return RedirectToAction("Index", new { id = model.NotificationId, type = model.SelectedType });
        }

        [HttpGet]
        public async Task<ActionResult> Delete(Guid id, Guid commentId, NotificationShipmentsCommentsType type)
        {
            var comments = await this.mediator.SendAsync(new GetImportNotificationComments(id, type, 1, null, null, null));

            DeleteCommentViewModel model = new DeleteCommentViewModel()
            {
                NotificationId = id,
                CommentId = commentId,
                Comment = comments.NotificationComments.FirstOrDefault(p => p.CommentId == commentId),
                Type = type
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(DeleteCommentViewModel model)
        {
            var request = new DeleteImportNotificationComment(model.CommentId);

            await this.mediator.SendAsync(request);

            return RedirectToAction("Index", new { id = model.NotificationId, type = model.Type });
        }
    }
}