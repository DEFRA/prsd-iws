namespace EA.Iws.Web.Areas.Admin.Controllers
{
    using EA.Iws.Requests.Notification;
    using EA.Iws.Web.Areas.Admin.ViewModels.ArchiveNotification;
    using EA.Iws.Web.Infrastructure.Authorization;
    using EA.Prsd.Core.Mediator;
    using Newtonsoft.Json;
    using Requests.Admin.ArchiveNotification;
    using System;
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
        public JsonResult SelectSingleNotification(List<NotificationArchiveSummaryData> selectedNotificationData, bool isChecked)
        {
            var selectNotificationList = new List<NotificationArchiveSummaryData>();
            if (TempData["SelectedNotifications"] != null)
            {
                selectNotificationList = JsonConvert.DeserializeObject<List<NotificationArchiveSummaryData>>(TempData["SelectedNotifications"].ToString());
            }

            if (isChecked)
            {
                var findAny = selectNotificationList.SingleOrDefault(x => x.Id == selectedNotificationData[0].Id);
                if (findAny == null)
                {
                    selectNotificationList.Add(selectedNotificationData[0]);
                }
            }
            else
            {
                var findAny = selectNotificationList.SingleOrDefault(x => x.Id == selectedNotificationData[0].Id);
                if (findAny != null)
                {
                    selectNotificationList.RemoveAll(s => s.Id == selectedNotificationData[0].Id);
                }
            }

            TempData["SelectedNotifications"] = JsonConvert.SerializeObject(selectNotificationList);
            var response = GetResonseResult(selectNotificationList);

            return Json(response);
        }

        [HttpPost]
        public JsonResult SelectAllNotifications(List<NotificationArchiveSummaryData> selectedNotificationsData, bool isChecked)
        {
            var selectNotificationList = new List<NotificationArchiveSummaryData>();
            if (isChecked)
            {
                foreach (var notification in selectedNotificationsData)
                {
                    selectNotificationList.Add(notification);
                }
            }
            else
            {
                selectNotificationList.Clear();
            }

            TempData["SelectedNotifications"] = JsonConvert.SerializeObject(selectNotificationList);
            var response = GetResonseResult(selectNotificationList);

            return Json(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ArchiveNotificationResultViewModel model)
        {
            var selectNotificationList = new List<NotificationArchiveSummaryData>();
            if (TempData["SelectedNotifications"] != null)
            {
                selectNotificationList = JsonConvert.DeserializeObject<List<NotificationArchiveSummaryData>>(TempData["SelectedNotifications"].ToString());
            }

            if (!ModelState.IsValid)
            {
                var reviewModel = new ArchiveNotificationReviewViewModel()
                {
                    SelectedNotifications = selectNotificationList
                };

                return View("Review", reviewModel);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Review(ArchiveNotificationReviewViewModel reviewModel)
        {
            return View(reviewModel);
        }

        [HttpGet]
        public ActionResult Remove(Guid notificationId)
        {
            var selectNotificationList = new List<NotificationArchiveSummaryData>();
            if (TempData["SelectedNotifications"] != null)
            {
                selectNotificationList = JsonConvert.DeserializeObject<List<NotificationArchiveSummaryData>>(TempData["SelectedNotifications"].ToString());
            }

            var findAny = selectNotificationList.SingleOrDefault(x => x.Id == notificationId);
            if (findAny != null)
            {
                selectNotificationList.RemoveAll(s => s.Id == notificationId);
            }

            TempData["SelectedNotifications"] = JsonConvert.SerializeObject(selectNotificationList);
            var response = GetResonseResult(selectNotificationList);

            var reviewModel = new ArchiveNotificationReviewViewModel()
            {
                SelectedNotifications = selectNotificationList
            };

            return View(reviewModel);
        }

        private async Task<ArchiveNotificationResultViewModel> GetUserArchiveNotifications(int pageNumber = 1)
        {
            var response = await mediator.SendAsync(new GetArchiveNotificationsByUser(pageNumber));
            var model = new ArchiveNotificationResultViewModel(response);
            var selectNotificationList = new List<NotificationArchiveSummaryData>();

            if (TempData["SelectedNotifications"] != null)
            {
                selectNotificationList = JsonConvert.DeserializeObject<List<NotificationArchiveSummaryData>>(TempData["SelectedNotifications"].ToString());
            }

            if (selectNotificationList != null && selectNotificationList.Count > 0)
            {
                foreach (var notification in selectNotificationList)
                {
                    var selectedNotification = model.Notifications.ToList().SingleOrDefault(x => x.Id == notification.Id);
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

            TempData["SelectedNotifications"] = JsonConvert.SerializeObject(selectNotificationList);

            return model;
        }

        private List<int> GetResonseResult(List<NotificationArchiveSummaryData> selectNotificationList)
        {
            List<int> returnList = new List<int>();

            foreach (var notification in selectNotificationList)
            {
                if (returnList.Count == 0)
                {
                    returnList.Add(notification.PageNumber.Value);
                }
                else
                {
                    var findAnyExits = returnList.Any(x => x.Equals(notification.PageNumber.Value));
                    if (findAnyExits == false)
                    {
                        returnList.Add(notification.PageNumber.Value);
                    }
                }
            }

            var response = new List<int>();
            response.Add(returnList.Count);
            response.Add(selectNotificationList.Count);

            return response;
        }
    }
}