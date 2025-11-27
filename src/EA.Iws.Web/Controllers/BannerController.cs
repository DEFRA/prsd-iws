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

        // GET: Banner
        [HttpGet]
        [AllowAnonymous]
        public ActionResult MessageBanner()
        {
            var messageBannerData = Task.Run(() => mediator.SendAsync(new GetMessageBanner())).Result;

            if (messageBannerData != null)
            {
                var messageBannerViewModel = new MessageBannerViewModel()
                {
                    Title = messageBannerData.Title,
                    Description = messageBannerData.Description,
                    IsActive = true
                };

                return PartialView("_IwsMessageBanner", messageBannerViewModel);
            }

            return PartialView("_IwsMessageBanner", new MessageBannerViewModel() { IsActive = false });
        }
    }
}