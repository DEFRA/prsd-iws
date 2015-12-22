namespace EA.Iws.Requests.Notification
{
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class GeneratePostageLabel : IRequest<byte[]>
    {
        public CompetentAuthority CompetentAuthority { get; private set; }

        public GeneratePostageLabel(CompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }
    }
}
