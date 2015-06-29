namespace EA.Iws.RequestHandlers.WasteCodes
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.WasteCodes;
    using DataAccess;
    using Domain.Notification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.WasteCodes;

    internal class GetWasteCodesForNotificationHandler :
        IRequestHandler<GetWasteCodesForNotification, WasteCodeData[]>
    {
        private readonly IwsContext context;
        private readonly IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]> mapper;
        private readonly IMap<WasteCodeInfo, WasteCodeData[]> mapper2;

        public GetWasteCodesForNotificationHandler(IwsContext context, IMap<IEnumerable<WasteCodeInfo>, WasteCodeData[]> mapper, IMap<WasteCodeInfo, WasteCodeData[]> mapper2)
        {
            this.context = context;
            this.mapper = mapper;
            this.mapper2 = mapper2;
        }

        public async Task<WasteCodeData[]> HandleAsync(GetWasteCodesForNotification message)
        {
            var notification =
                await
                    context.NotificationApplications.SingleAsync(n => n.Id == message.NotificationId);

            switch (message.CodeType)
            {
                case CodeType.Ewc:
                    return mapper.Map(notification.EwcCodes);
                case CodeType.Basel:
                case CodeType.Oecd:
                    return mapper2.Map(notification.BaselOecdCode);
                case CodeType.ExportCode:
                    return mapper2.Map(notification.ExportCode);
                case CodeType.ImportCode:
                    return mapper2.Map(notification.ImportCode);
                case CodeType.OtherCode:
                    return mapper2.Map(notification.OtherCode);
                case CodeType.Y:
                    return mapper.Map(notification.YCodes);
                case CodeType.H:
                    return mapper.Map(notification.HCodes);
                case CodeType.Un:
                    return mapper.Map(notification.UnClasses);
                case CodeType.UnNumber:
                    return mapper.Map(notification.UnNumbers);
                case CodeType.CustomsCode:
                    return mapper.Map(notification.CustomsCodes);
                default:
                    throw new InvalidOperationException(string.Format("Unknown code type {0}", message.CodeType));
            }
        }
    }
}