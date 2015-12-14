namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using Core.Admin.Reports;
    using Core.Shared;
    using Domain;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class MissingShipmentMap : IMapWithParameter<MissingShipment, UKCompetentAuthority, MissingShipmentData>
    {
        private readonly IWorkingDayCalculator workingDayCalculator;

        public MissingShipmentMap(IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public MissingShipmentData Map(MissingShipment source, UKCompetentAuthority parameter)
        {
            return new MissingShipmentData
            {
                Importer = source.Importer,
                PrenotificationDate = source.PrenotificationDate,
                ReceivedDate = source.ReceivedDate,
                NotificationNumber = source.NotificationNumber,
                Exporter = source.Exporter,
                Facility = source.Facility,
                TonnesQuantityReceived = GetTonnes(source),
                CubicMetresQuantityReceived = GetCubicMetres(source),
                ActualDateOfShipment = source.ActualDateOfShipment,
                ChemicalComposition = source.ChemicalComposition,
                CompetentAuthorityArea = source.LocalArea,
                ConsentValidFrom = source.ConsentFrom,
                ConsentValidTo = source.ConsentTo,
                RecoveryOrDisposalDate = source.CompletedDate,
                ShipmentNumber = source.ShipmentNumber,
                WasPrenotifiedBeforeThreeWorkingDays = GetWasPrenotifiedThreeWorkingDaysBeforeActualDate(source, parameter)
            };
        }

        private decimal? GetTonnes(MissingShipment source)
        {
            if (!source.Units.HasValue 
                || !source.QuantityReceived.HasValue 
                || ShipmentQuantityUnitsMetadata.IsVolumeUnit(source.Units.Value))
            {
                return null;
            }

            return ShipmentQuantityUnitConverter.ConvertToTarget(source.Units.Value, 
                ShipmentQuantityUnits.Tonnes,
                source.QuantityReceived.Value,
                false);
        }

        private decimal? GetCubicMetres(MissingShipment source)
        {
            if (!source.Units.HasValue
                || !source.QuantityReceived.HasValue
                || ShipmentQuantityUnitsMetadata.IsWeightUnit(source.Units.Value))
            {
                return null;
            }

            return ShipmentQuantityUnitConverter.ConvertToTarget(source.Units.Value,
                ShipmentQuantityUnits.CubicMetres,
                source.QuantityReceived.Value,
                false);
        }

        private bool GetWasPrenotifiedThreeWorkingDaysBeforeActualDate(MissingShipment source, UKCompetentAuthority competentAuthority)
        {
            if (!source.PrenotificationDate.HasValue 
                || !source.ActualDateOfShipment.HasValue)
            {
                return false;
            }

            return workingDayCalculator.GetWorkingDays(source.PrenotificationDate.Value,
                source.ActualDateOfShipment.Value,
                false,
                competentAuthority) >= 3;
        }
    }
}