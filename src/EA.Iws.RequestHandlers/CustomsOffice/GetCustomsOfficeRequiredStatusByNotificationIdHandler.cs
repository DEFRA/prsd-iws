namespace EA.Iws.RequestHandlers.CustomsOffice
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Prsd.Core.Mediator;
    using Requests.CustomsOffice;
  
    internal class GetCustomsCompletionStatusByNotificationIdHandler : IRequestHandler<GetCustomsCompletionStatusByNotificationId, CustomsOfficeCompletionStatus>
    {
        private readonly IwsContext context;

        public GetCustomsCompletionStatusByNotificationIdHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<CustomsOfficeCompletionStatus> HandleAsync(GetCustomsCompletionStatusByNotificationId message)
        {
            var notification = await context.NotificationApplications.SingleAsync(n => n.Id == message.Id);

            return new CustomsOfficeCompletionStatus
            {
                CustomsOfficesCompleted = notification.GetCustomsOfficesCompleted(),
                CustomsOfficesRequired = notification.GetCustomsOfficesRequired()
            };
        }
    }
}
