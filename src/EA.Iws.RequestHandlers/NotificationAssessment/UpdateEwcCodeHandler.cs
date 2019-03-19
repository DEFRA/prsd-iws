﻿namespace EA.Iws.RequestHandlers.NotificationAssessment
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationAssessment;

    public class UpdateEwcCodeHandler : IRequestHandler<UpdateEwcCode, bool>
    {
        private readonly IwsContext context;

        public UpdateEwcCodeHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<bool> HandleAsync(UpdateEwcCode message)
        {
            var notification = await context.GetNotificationApplication(message.Id);

            if (message.IsNotApplicable)
            {
                notification.SetCodesNotApplicable(CodeType.Ewc);
            }
            else
            {
                var codes = await context.WasteCodes.Where(wc => message.Codes.Contains(wc.Id)).ToArrayAsync();

                notification.SetEwcCodes(codes.Select(WasteCodeInfo.CreateWasteCodeInfo));
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
