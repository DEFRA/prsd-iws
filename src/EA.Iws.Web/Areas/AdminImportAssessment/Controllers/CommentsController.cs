namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Core.Admin;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.ImportNotificationAssessment;
    using EA.Iws.Web.ViewModels.Shared;
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
                ShipmentNumberStr = shipmentNumber.ToString(),
                PageNumber = comments.PageNumber,
                PageSize = comments.PageSize,
                TotalNumberOfFilteredComments = comments.NumberOfFilteredComments,
                Comments = comments.NotificationComments.OrderBy(p => p.ShipmentNumber).ThenBy(p => p.DateAdded).ToList()
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

            PrepareModelErrors(model.SelectedFilter, model);

            if (command == "search" && !ModelState.IsValid)
            {
                var comments = await this.mediator.SendAsync(new GetImportNotificationComments(id, model.Type, model.PageNumber, null, null, null));
                model.TotalNumberOfComments = comments.NumberOfComments;
                model.Comments = comments.NotificationComments.ToList();

                return View(model);
            }

            if (model.SelectedFilter == "date")
            {
                DateTime startDate = new DateTime(model.From.Year.GetValueOrDefault(), model.From.Month.GetValueOrDefault(), model.From.Day.GetValueOrDefault());
                DateTime endDate = new DateTime(model.To.Year.GetValueOrDefault(), model.To.Month.GetValueOrDefault(), model.To.Day.GetValueOrDefault());

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

        private void PrepareModelErrors(string filter, CommentsViewModel model)
        {
            if (filter == "shipment")
            {
                // Get all of the date property names and then remove them from the modelstate errors
                List<string> properties = GetViewModelDatePropertyNames();
                foreach (string name in properties)
                {
                    if (ModelState.ContainsKey(name))
                    {
                        ModelState[name].Errors.Clear();
                    }
                }

                // Check that the shipment number has been filled in properly
                if (model.ShipmentNumber == null || model.ShipmentNumber < 1 || model.ShipmentNumber.ToString().Length > 6)
                {
                    // If there is already an error for the shipment number, it's because the model has added one in the validation method, so don't add another.
                    ModelState shipmentNumberErrors;
                    ModelState.TryGetValue("ShipmentNumber", out shipmentNumberErrors);
                    if (shipmentNumberErrors == null || shipmentNumberErrors.Errors.Count == 0)
                    {
                        ModelState.AddModelError("shipmentNumberStr", "Enter a valid shipment number");
                    }
                }
            }
            else if (filter == "date")
            {
                if (ModelState.ContainsKey("shipmentNumberStr"))
                {
                    ModelState["shipmentNumberStr"].Errors.Clear();
                }
            }
        }

        private List<string> GetViewModelDatePropertyNames()
        {
            List<string> datePropertyNames = new List<string>();

            PropertyInfo[] commentsViewModelPropertyInfos;
            commentsViewModelPropertyInfos = typeof(CommentsViewModel).GetProperties();

            foreach (var x in commentsViewModelPropertyInfos.Where(p => p.Name == "To" || p.Name == "From"))
            {
                PropertyInfo[] dateEntryViewModelPropertyInfos;
                dateEntryViewModelPropertyInfos = typeof(DateEntryViewModel).GetProperties();

                foreach (var y in dateEntryViewModelPropertyInfos.Where(p => p.Name == "Day" || p.Name == "Month" || p.Name == "Year"))
                {
                    string name = string.Format("{0}.{1}", x.Name, y.Name);
                    datePropertyNames.Add(name);
                }
            }

            return datePropertyNames;
        }
    }
}