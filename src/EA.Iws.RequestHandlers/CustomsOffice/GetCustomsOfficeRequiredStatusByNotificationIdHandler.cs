namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;

    internal class GetCustomsCompletionStatusByNotificationIdHandler :
        IRequestHandler<GetCustomsCompletionStatusByNotificationId, CustomsOfficeCompletionStatus>
    {
        private readonly IwsContext context;

        public GetCustomsCompletionStatusByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<CustomsOfficeCompletionStatus> HandleAsync(GetCustomsCompletionStatusByNotificationId message)
        {
            await context.CheckNotificationAccess(message.Id);

            var transportRoute = await context.TransportRoutes.SingleAsync(p => p.NotificationId == message.Id);

            return new CustomsOfficeCompletionStatus
            {
                CustomsOfficesRequired = transportRoute.GetRequiredCustomsOffices()
            };
        }
    }
}