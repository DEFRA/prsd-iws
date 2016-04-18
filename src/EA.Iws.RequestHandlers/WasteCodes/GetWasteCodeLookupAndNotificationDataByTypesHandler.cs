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
        private readonly IMap<IEnumerable<WasteCode>, WasteCodeData[]> wasteCodeMapper;
        private readonly IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]> wasteCodeInfoMapper;

        public GetWasteCodeLookupAndNotificationDataByTypesHandler(IwsContext context, 
            IMap<IEnumerable<WasteCode>, WasteCodeData[]> wasteCodeMapper,
            IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]> wasteCodeInfoMapper)
        {
            this.context = context;
            this.wasteCodeMapper = wasteCodeMapper;
            this.wasteCodeInfoMapper = wasteCodeInfoMapper;
        }

        public async Task<WasteCodeDataAndNotificationData> HandleAsync(GetWasteCodeLookupAndNotificationDataByTypes message)
        {
            var notification = await context.NotificationApplications.SingleAsync(na => na.Id == message.Id);

            IList<WasteCode> lookupCodes = await GetLookupCodes(message);
            IList<WasteCodeInfo> notificationCodes = GetNotificationCodes(message, notification);
            
            var lookupCodesDictionary = lookupCodes.GroupBy(wc => wc.CodeType).ToDictionary(x => x.Key, x => wasteCodeMapper.Map(x));

            var notificationCodesDictionary = message.NotificationWasteCodeTypes.GroupBy(wc => wc)
                .ToDictionary(x => x.Key, x => wasteCodeInfoMapper.Map(notificationCodes.Where(nc => nc.CodeType == x.Key)));

            var notApplicable =
                notification.WasteCodes.Where(wc => wc.IsNotApplicable).Select(wc => wc.CodeType).ToList();

            return new WasteCodeDataAndNotificationData
            {
                LookupWasteCodeData = lookupCodesDictionary,
                NotificationWasteCodeData = notificationCodesDictionary,
                NotApplicableCodes = notApplicable
            };
        }

        private IList<WasteCodeInfo> GetNotificationCodes(GetWasteCodeLookupAndNotificationDataByTypes message, NotificationApplication notification)
        {
            if (message.NotificationWasteCodeTypes.Count == 0)
            {
                return notification.WasteCodes.Where(wc => wc.WasteCode != null).ToArray();
            }

            return notification.WasteCodes
                .Where(wc => message.NotificationWasteCodeTypes.Contains(wc.CodeType))
                             .ToArray();
        }

        private async Task<IList<WasteCode>> GetLookupCodes(GetWasteCodeLookupAndNotificationDataByTypes message)
        {
            if (message.LookupWasteCodeTypes.Count == 0)
            {
                return new WasteCode[0];
            }

            return await
                context.WasteCodes.Where(wc => message.LookupWasteCodeTypes.Contains(wc.CodeType))
                    .ToArrayAsync();
        }
    }
}
