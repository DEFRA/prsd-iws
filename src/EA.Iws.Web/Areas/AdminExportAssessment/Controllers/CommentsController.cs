namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using Core.Authorization.Permissions;
    using Infrastructure.Authorization;
    using ViewModels.Comments;

    [AuthorizeActivity(ExportNotificationPermissions.CanEditInternalComments)]
    public class CommentsController : Controller
    {
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
        public ActionResult Add(AddCommentsViewModel m)
        {
            if (!ModelState.IsValid)
            {
                m.ModelIsValid = false;
                return View(m);
            }
            AddCommentsViewModel model = new AddCommentsViewModel();

            return View(model);
        }
    }
}