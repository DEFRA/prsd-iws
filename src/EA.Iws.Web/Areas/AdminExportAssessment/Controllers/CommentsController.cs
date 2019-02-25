namespace EA.Iws.Web.Areas.AdminExportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using ViewModels.Comments;

    [Authorize(Roles = "internal,readonly")]
    public class CommentsController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            CommentsViewModel model = new CommentsViewModel();
            model.NotificationId = id;
            return View(model);
        }
    }
}