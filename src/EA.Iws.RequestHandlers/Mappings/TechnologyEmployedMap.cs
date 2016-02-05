namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.OperationCodes;
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class TechnologyEmployedMap : IMap<NotificationApplication, TechnologyEmployedData>
    {
        public TechnologyEmployedData Map(NotificationApplication source)
        {
            if (source.HasTechnologyEmployed)
            {
                return new TechnologyEmployedData
                {
                    AnnexProvided = source.TechnologyEmployed.AnnexProvided,
                    Details = source.TechnologyEmployed.Details,
                    NotificationId = source.Id,
                    FurtherDetails = source.TechnologyEmployed.FurtherDetails,
                    HasTechnologyEmployed = true,
                    OperationCodes = GetOperationCodes(source)
                };
            }

            return new TechnologyEmployedData
            {
                NotificationId = source.Id,
                HasTechnologyEmployed = false,
                OperationCodes = GetOperationCodes(source)
            };
        }

        private IList<OperationCode> GetOperationCodes(NotificationApplication notification)
        {
            if (notification == null || notification.OperationInfos == null)
            {
                return new List<OperationCode>();
            }

            return notification.OperationInfos.Select(o => o.OperationCode).ToList();
        }
    }
}