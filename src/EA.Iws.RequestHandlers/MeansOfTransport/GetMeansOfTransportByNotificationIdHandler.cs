namespace EA.Iws.RequestHandlers.MeansOfTransport
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.MeansOfTransport;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.MeansOfTransport;

    internal class GetMeansOfTransportByNotificationIdHandler : IRequestHandler<GetMeansOfTransportByNotificationId, IList<MeansOfTransport>>
    {
        private readonly IwsContext context;

        public GetMeansOfTransportByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<IList<MeansOfTransport>> HandleAsync(GetMeansOfTransportByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);

            return notification.MeansOfTransport.ToArray();
        }
    }
}
