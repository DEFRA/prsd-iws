namespace EA.Iws.Requests.MessageBanner
{
    using EA.Iws.Core.MessageBanner;
    using EA.Prsd.Core.Mediator;
    using EA.Prsd.Core.Security;

    [AllowUnauthorizedUser]
    public class GetMessageBanner : IRequest<MessageBannerData>
    {
        public GetMessageBanner()
        {
        }
    }
}
