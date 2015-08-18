namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class GetWasteCodeLookupAndNotificationDataByTypesHandler : IRequestHandler<GetWasteCodeLookupAndNotificationDataByTypes, WasteCodeDataAndNotificationData>
    {
        private readonly IwsContext context;
        private readonly IMap<IEnumerable<WasteCode>, WasteCodeData[]> mapper;

        public GetWasteCodeLookupAndNotificationDataByTypesHandler(IwsContext context, 
            IMap<IEnumerable<WasteCode>, WasteCodeData[]> mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<WasteCodeDataAndNotificationData> HandleAsync(GetWasteCodeLookupAndNotificationDataByTypes message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.Id);

            IList<WasteCode> lookupCodes = await GetLookupCodes(message);
            IList<WasteCode> notificationCodes = GetNotificationCodes(message, notification);
            
            var lookupCodesDictionary = lookupCodes.GroupBy(wc => wc.CodeType).ToDictionary(x => x.Key, x => mapper.Map(x));

            var notificationCodesDictionary = message.NotificationWasteCodeTypes.GroupBy(wc => wc)
                .ToDictionary(x => x.Key, x => mapper.Map(notificationCodes.Where(nc => nc.CodeType == x.Key)));

            var notApplicable =
                notification.WasteCodes.Where(wc => wc.IsNotApplicable).Select(wc => wc.CodeType).ToList();

            return new WasteCodeDataAndNotificationData
            {
                LookupWasteCodeData = lookupCodesDictionary,
                NotificationWasteCodeData = notificationCodesDictionary,
                NotApplicableCodes = notApplicable
            };
        }

        private IList<WasteCode> GetNotificationCodes(GetWasteCodeLookupAndNotificationDataByTypes message, NotificationApplication notification)
        {
            if (message.NotificationWasteCodeTypes.Count == 0)
            {
                return notification.WasteCodes.Where(wc => wc.WasteCode != null).Select(wc => wc.WasteCode).ToArray();
            }

            return notification.WasteCodes
                .Where(wc => wc.WasteCode != null 
                    && message.NotificationWasteCodeTypes.Contains(wc.WasteCode.CodeType))
                .Select(wc => wc.WasteCode)
                .ToArray();
        }

        private async Task<IList<WasteCode>> GetLookupCodes(GetWasteCodeLookupAndNotificationDataByTypes message)
        {
            if (message.LookupWasteCodeTypes.Count == 0)
            {
                return await context.WasteCodes.ToArrayAsync();
            }

            return await
                context.WasteCodes.Where(wc => message.LookupWasteCodeTypes.Contains(wc.CodeType))
                    .ToArrayAsync();
        }
    }
}
