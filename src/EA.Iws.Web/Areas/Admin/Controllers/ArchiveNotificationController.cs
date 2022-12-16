namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using Requests.Admin.ArchiveNotification;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    [AuthorizeActivity(typeof(ArchiveNotifications))]
    public class ArchiveNotificationController : Controller
    {
        private readonly IMediator mediator;

        public ArchiveNotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Index(int page = 1)
        {
            var model = await GetUserArchiveNotifications(page);
            return View(model);
        }        

        [HttpPost]
        public JsonResult SelectedNotification(string notificationId, bool isChecked)
        {
            var selectNotificationList = (List<string>)TempData["SelectedNotificationList"] ?? new List<string>();
            if (isChecked && !selectNotificationList.Contains(notificationId))
            {
                selectNotificationList.Add(notificationId);
            }
            else if (!isChecked && selectNotificationList.Contains(notificationId))
            {
                selectNotificationList.RemoveAll(s => s == notificationId);
            }

            TempData["SelectedNotificationList"] = selectNotificationList;

            return Json(selectNotificationList.Count);
        }

        [HttpPost]
        public JsonResult SelectedAllNotifications(List<string> notificationIds, bool isChecked)
        {
            var selectNotificationList = new List<string>();
            if (isChecked)
            {
                foreach (var notificationId in notificationIds)
                {
                    selectNotificationList.Add(notificationId);
                }
            }
            else
            {
                selectNotificationList.Clear();
            }

            TempData["SelectedNotificationList"] = selectNotificationList;

            return Json(selectNotificationList.Count);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(ArchiveNotificationResultViewModel model)
        {
            var selectNotificationList = (List<string>)TempData["SelectedNotificationList"] ?? new List<string>();
            if (selectNotificationList == null || selectNotificationList.Count == 0)
            {
                model.HasAnyNotificationSelected = false;
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return View(model);
        }

        private async Task<ArchiveNotificationResultViewModel> GetUserArchiveNotifications(int pageNumber = 1)
        {
            var response = await mediator.SendAsync(new GetArchiveNotificationsByUser(pageNumber));
            var model = new ArchiveNotificationResultViewModel(response);

            //var selectNotificationList = (List<string>)HttpContext.Session["SelectedNotificationList"] ?? new List<string>();
            var selectNotificationList = (List<string>)TempData["SelectedNotificationList"] ?? new List<string>();
            if (selectNotificationList != null && selectNotificationList.Count > 0)
            {
                foreach (var notification in selectNotificationList)
                {
                    var selectedNotification = model.Notifications.ToList().SingleOrDefault(x => x.NotificationNumber == notification);
                    if (selectedNotification != null)
                    {
                        selectedNotification.IsSelected = true;
                    }
                }

                model.NumberOfNotificationsSelected = selectNotificationList.Count;

                var isAnyUnChecked = model.Notifications.Any(x => x.IsSelected == false);
                if (isAnyUnChecked)
                {
                    model.IsSelectAllChecked = false;
                }
                else
                {
                    model.IsSelectAllChecked = true;
                }
            }
            else
            {
                model.NumberOfNotificationsSelected = 0;
            }

            TempData["SelectedNotificationList"] = selectNotificationList;

            return model;
        }
    }
}