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

    internal class SetUNClassesHandler : IRequestHandler<SetUNClasses, bool>
    {
        private readonly IwsContext context;

        public SetUNClassesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetUNClasses message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            if (message.IsNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.Un);
            }
            else
            {
                var codes = await context.WasteCodes.Where(wc => message.Codes.Contains(wc.Id)).ToArrayAsync();

                notification.SetUnClasses(codes.Select(WasteCodeInfo.CreateWasteCodeInfo));
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
