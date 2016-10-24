namespace EA.Iws.RequestHandlers.Mappings
{
    using Core.TechnologyEmployed;
    using Domain.NotificationApplication;
    using Prsd.Core.Mapper;

    internal class TechnologyEmployedMap : IMap<TechnologyEmployed, TechnologyEmployedData>
    {
        public TechnologyEmployedData Map(TechnologyEmployed source)
        {
            if (source != null)
            {
                return new TechnologyEmployedData
                {
                    AnnexProvided = source.AnnexProvided,
                    Details = source.Details,
                    NotificationId = source.NotificationId,
                    FurtherDetails = source.FurtherDetails,
                    HasTechnologyEmployed = true,
                };
            }

            return new TechnologyEmployedData
            {
                HasTechnologyEmployed = false
            };
        }
    }
}