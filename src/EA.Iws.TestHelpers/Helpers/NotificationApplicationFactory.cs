namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.MeansOfTransport;
    using Core.RecoveryInfo;
    using Core.WasteCodes;
    using Domain;
    using Domain.Notification;
    using Domain.TransportRoute;
    using OI = ObjectInstantiator<Domain.Notification.NotificationApplication>; 

    public class NotificationApplicationFactory
    {
        public static NotificationApplication Create(Guid id, int number = 250)
        {
            var notificationApplication = new NotificationApplication(Guid.Empty, NotificationType.Recovery,
                UKCompetentAuthority.England, number);

            EntityHelper.SetEntityId(notificationApplication, id);

            return notificationApplication;
        }

        public static NotificationApplication CreateCompleted(Guid id, 
            Guid userId,
            IList<Country> countries, 
            IList<EntryOrExitPoint> entryOrExitPoints, 
            IList<CompetentAuthority> competentAuthorities, 
            IList<WasteCode> wasteCodes,
            int number = 250)
        {
            var notification = Create(id, number);
            OI.SetProperty(x => x.UserId, userId, notification);
            OI.SetProperty(x => x.Exporter, ExporterFactory.Create(Guid.NewGuid()), notification);
            OI.SetProperty(x => x.Importer, ImporterFactory.Create(Guid.NewGuid()), notification);
            notification.AddProducer(ComplexTypeFactory.Create<ProducerBusiness>("business1"), ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddProducer(ComplexTypeFactory.Create<ProducerBusiness>("business2"), ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddCarrier(ComplexTypeFactory.Create<Business>("carrier1"), ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddCarrier(ComplexTypeFactory.Create<Business>("carrier2"), ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());
            notification.AddFacility(ComplexTypeFactory.Create<Business>(), ComplexTypeFactory.Create<Address>(),
                ComplexTypeFactory.Create<Contact>());

            OI.SetProperty(x => x.PercentageRecoverable, 100, notification);
            
            notification.SetMeansOfTransport(new List<MeansOfTransport> { MeansOfTransport.Air, MeansOfTransport.Road });
            notification.SetPhysicalCharacteristics(new List<PhysicalCharacteristicsInfo> { PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Sludgy) });
            notification.SetShipmentInfo(new DateTime(DateTime.UtcNow.Year + 1, 2, 1), new DateTime(DateTime.UtcNow.Year + 2, 1, 1), 420, 69, ShipmentQuantityUnits.Kilogram);

            var exitPoint = entryOrExitPoints.OrderBy(ep => ep.Country.Name).First(ep => ep.Country.IsEuropeanUnionMember);
            var stateOfExport = new StateOfExport(exitPoint.Country,
                competentAuthorities.First(ca => ca.Country.Id == exitPoint.Country.Id), exitPoint);

            var entryPoint = entryOrExitPoints.OrderBy(ep => ep.Country.Name)
                    .First(ep => ep.Country.IsEuropeanUnionMember && ep.Country.Id != exitPoint.Country.Id);
            var stateOfImport = new StateOfImport(entryPoint.Country, competentAuthorities.First(ca => ca.Country.Id == entryPoint.Country.Id), entryPoint);

            notification.SetStateOfExportForNotification(stateOfExport);
            notification.SetStateOfImportForNotification(stateOfImport);

            notification.AddWasteType(WasteType.CreateOtherWasteType("boules"));
            notification.SetTechnologyEmployed(TechnologyEmployed.CreateTechnologyEmployedDetails("something cheddar gorge"));
            notification.SetRecoveryInfoValues(RecoveryInfoUnits.Kilogram, 10, RecoveryInfoUnits.Kilogram, 10, null,
                null);
            notification.SetOperationCodes(new[] { OperationCode.R1, OperationCode.R7 });
            notification.ReasonForExport = "recovery";
            notification.SetEwcCodes(new[] { WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Ewc)) });
            notification.SetHCodes(new[] { WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.H)) });
            notification.SetYCodes(new[] { WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Y)) });
            notification.SetUnClasses(new[] { WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Un)) });
            notification.SetUnNumbers(new[] { WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.UnNumber)) });
            notification.SetCustomsCodes(new[] { WasteCodeInfo.CreateCustomWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.CustomsCode), "olives") });
            notification.SetImportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.ImportCode), "impomptant"));
            notification.SetExportCode(WasteCodeInfo.CreateCustomWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.ExportCode), "woohoo"));
            notification.SetPreconsentedRecoveryFacility(false);

            return notification;
        }
    }
}
