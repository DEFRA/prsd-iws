namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class SetHCodesHandler : IRequestHandler<SetHCodes, bool>
    {
        private readonly IwsContext context;

        public SetHCodesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetHCodes message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            if (message.IsNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.H);
            }
            else
            {
                var codes = await context.WasteCodes.Where(wc => message.Codes.Contains(wc.Id)).ToArrayAsync();

                notification.SetHCodes(codes.Select(WasteCodeInfo.CreateWasteCodeInfo));
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
