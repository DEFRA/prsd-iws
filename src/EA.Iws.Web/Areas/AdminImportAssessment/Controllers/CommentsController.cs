namespace EA.Iws.Web.Areas.AdminImportAssessment.Controllers
{
    using System;
    using System.Web.Mvc;
    using ViewModels.Comments;

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