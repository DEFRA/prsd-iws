using EA.Iws.Core.Carriers;
using EA.Iws.Core.OperationCodes;
using EA.Iws.Core.WasteCodes;
using EA.Iws.Domain.NotificationApplication;
using EA.Iws.Domain.OperationCodes;
using EA.Iws.Requests.OperationCodes;
using EA.Iws.Requests.WasteCodes;
using EA.Prsd.Core.Mapper;
using EA.Prsd.Core.Mediator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OperationCode = EA.Iws.Domain.OperationCodes.OperationCode;

namespace EA.Iws.RequestHandlers.OperationCodes
{
    internal class GetOperationCodesByNotificationTypeHandler : IRequestHandler<GetOperationCodesByNotificationType, OperationCodeData[]>
    {
        private readonly IOperationCodeRepository operationCodeRepository;
        private readonly IMap<OperationCode, IList<OperationCodeData>> mapper;

        public GetOperationCodesByNotificationTypeHandler(IOperationCodeRepository operationCodeRepository)
        {
            this.operationCodeRepository = operationCodeRepository;
        }

        public async Task<OperationCodeData[]> HandleAsync(GetOperationCodesByNotificationType message)
        {
            IEnumerable<OperationCode> result = await operationCodeRepository.GetOperationCodes(message.NotificationType, message.IsInterim);

            return mapper.Map(result.SingleOrDefault());
        }
    }
}
