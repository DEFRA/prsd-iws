namespace EA.Iws.Web.Controllers
{
    using EA.Iws.Requests.MessageBanner;
    using EA.Iws.Web.ViewModels.Shared;
    using EA.Prsd.Core.Mediator;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class BannerController : Controller
    {
        private readonly IMediator mediator;

        public BannerController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> MessageBannerAsync()
        {
            var messageBannerData = await mediator.SendAsync(new GetMessageBanner());

            if (messageBannerData != null)
            {
                var messageBannerViewModel = new MessageBannerViewModel()
                {
                    Title = messageBannerData.Title,
                    Description = messageBannerData.Description,
                    IsActive = true
                };

                return Json(messageBannerViewModel, JsonRequestBehavior.AllowGet);
            }

            return Json(new MessageBannerViewModel() { IsActive = false }, JsonRequestBehavior.AllowGet);
        }
    }
}