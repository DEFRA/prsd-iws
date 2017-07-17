namespace EA.Iws.RequestHandlers.Mappings.Reports
{
    using Core.Admin.Reports;
    using Core.Notification;
    using Core.Shared;
    using Domain;
    using Domain.Reports;
    using Prsd.Core.Mapper;

    internal class MissingShipmentMap : IMapWithParameter<MissingShipment, UKCompetentAuthority, ShipmentData>
    {
        private readonly IWorkingDayCalculator workingDayCalculator;

        public MissingShipmentMap(IWorkingDayCalculator workingDayCalculator)
        {
            this.workingDayCalculator = workingDayCalculator;
        }

        public ShipmentData Map(MissingShipment source, UKCompetentAuthority parameter)
        {
            return new ShipmentData
            {
                Importer = source.Importer,
                PrenotificationDate = source.PrenotificationDate,
                ReceivedDate = source.ReceivedDate,
                NotificationNumber = source.NotificationNumber,
                Exporter = source.Exporter,
                Facility = source.Facility,
                TonnesQuantityReceived = GetActualTonnes(source),
                CubicMetresQuantityReceived = GetActualCubicMetres(source),
                ActualDateOfShipment = source.ActualDateOfShipment,
                ChemicalComposition = source.ChemicalComposition,
                CompetentAuthorityArea = source.LocalArea,
                ConsentValidFrom = source.ConsentFrom,
                ConsentValidTo = source.ConsentTo,
                RecoveryOrDisposalDate = source.CompletedDate,
                ShipmentNumber = source.ShipmentNumber,
                WasPrenotifiedBeforeThreeWorkingDays = GetWasPrenotifiedThreeWorkingDaysBeforeActualDate(source, parameter),
                CancelledShipment = source.Status == "Cancelled" ? "Cancelled" : string.Empty,
                PortOfEntry = source.EntryPort,
                PortOfExit = source.ExitPort,
                DestinationCountry = source.DestinationCountry,
                DispatchingCountry = source.OriginatingCountry,
                IntendedTonnesQuantity = GetIntendedTonnes(source),
                IntendedCubicMetresQuantity = GetIntendedCubicMetres(source),
                EwcCodes = source.EwcCodes
            };
        }

        private decimal? GetActualTonnes(MissingShipment source)
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

        private decimal? GetActualCubicMetres(MissingShipment source)
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

        private decimal? GetIntendedTonnes(MissingShipment source)
        {
            if (!source.TotalQuantityUnitsId.HasValue
                || !source.TotalQuantity.HasValue
                || ShipmentQuantityUnitsMetadata.IsVolumeUnit(source.TotalQuantityUnitsId.Value))
            {
                return null;
            }

            return ShipmentQuantityUnitConverter.ConvertToTarget(source.TotalQuantityUnitsId.Value,
                ShipmentQuantityUnits.Tonnes,
                source.TotalQuantity.Value,
                false);
        }

        private decimal? GetIntendedCubicMetres(MissingShipment source)
        {
            if (!source.TotalQuantityUnitsId.HasValue
                || !source.TotalQuantity.HasValue
                || ShipmentQuantityUnitsMetadata.IsWeightUnit(source.TotalQuantityUnitsId.Value))
            {
                return null;
            }

            return ShipmentQuantityUnitConverter.ConvertToTarget(source.TotalQuantityUnitsId.Value,
                ShipmentQuantityUnits.CubicMetres,
                source.TotalQuantity.Value,
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