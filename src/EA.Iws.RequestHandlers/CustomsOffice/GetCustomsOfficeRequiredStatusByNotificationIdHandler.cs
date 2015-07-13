namespace EA.Iws.RequestHandlers.CustomsOffice
{
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
            var notification = await context.GetNotificationApplication(message.Id);

            return new CustomsOfficeCompletionStatus
            {
                CustomsOfficesCompleted = notification.GetCustomsOfficesCompleted(),
                CustomsOfficesRequired = notification.GetCustomsOfficesRequired()
            };
        }
    }
}