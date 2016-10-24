namespace EA.Iws.TestHelpers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Core.MeansOfTransport;
    using Core.Notification;
    using Core.OperationCodes;
    using Core.Shared;
    using Core.WasteCodes;
    using Core.WasteType;
    using Domain;
    using Domain.NotificationApplication;
    using OI = ObjectInstantiator<Domain.NotificationApplication.NotificationApplication>;

    public static class NotificationApplicationFactory
    {
        public static readonly string ProducerBusinessName1 = "business1";
        public static readonly string ProducerBusinessName2 = "business2";
        public static readonly string CarrierBusinessName1 = "carrier1";
        public static readonly string CarrierBusinessName2 = "carrier2";
        public static readonly string FacilityBusinessName1 = "facility1";
        public static readonly string FacilityBusinessName2 = "facility2";
        public static readonly string OtherWasteCode = "baubles";

        public static NotificationApplication Create(Guid id, int number = 250)
        {
            var notificationApplication = Activator.CreateInstance(
                type: typeof(NotificationApplication),
                bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance,
                culture: null,
                binder: null,
                args: new object[] 
                {
                    Guid.Empty,
                    NotificationType.Recovery,
                    UKCompetentAuthority.England,
                    number
                }) as NotificationApplication;

            EntityHelper.SetEntityId(notificationApplication, id);

            return notificationApplication;
        }

        public static NotificationApplication Create(Guid userId, NotificationType notificationType,
            UKCompetentAuthority competentAuthority, int number)
        {
            var notificationApplication = Activator.CreateInstance(
                type: typeof(NotificationApplication),
                bindingAttr: BindingFlags.NonPublic | BindingFlags.Instance,
                culture: null,
                binder: null,
                args: new object[]
                {
                    userId,
                    notificationType,
                    competentAuthority,
                    number
                }) as NotificationApplication;

            return notificationApplication;
        }

        public static NotificationApplication CreateCompleted(Guid id,
            Guid userId,
            IList<Country> countries,
            IList<WasteCode> wasteCodes,
            int number = 250)
        {
            var notification = Create(id, number);

            OI.SetProperty(x => x.UserId, userId, notification);
            
            notification.SetPhysicalCharacteristics(new List<PhysicalCharacteristicsInfo>
            {
                PhysicalCharacteristicsInfo.CreatePhysicalCharacteristicsInfo(PhysicalCharacteristicType.Sludgy)
            });

            notification.SetWasteType(WasteType.CreateRdfWasteType(new[]
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation("boulder", 5, 10, WasteInformationType.Energy),
                WasteAdditionalInformation.CreateWasteAdditionalInformation("notes", 6, 9, WasteInformationType.AshContent)
            }));

            SetWasteCodes(notification, wasteCodes);

            SetProperty("WasteAdditionalInformationCollection", new List<WasteAdditionalInformation>(),
                notification.WasteType);
            notification.SetWasteAdditionalInformation(new[]
            {
                WasteAdditionalInformation.CreateWasteAdditionalInformation("Rubik's cubes", 1, 10,
                    WasteInformationType.AshContent)
            });

            notification.SetOperationCodes(new[] { OperationCode.R1, OperationCode.R7 });
            notification.ReasonForExport = "recovery";

            return notification;
        }

        private static void SetProperty<T>(string name, object value, T target)
        {
            var prop = typeof(T).GetProperty(name,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            prop.SetValue(target, value, null);
        }

        private static void SetWasteCodes(NotificationApplication notification, IList<WasteCode> wasteCodes)
        {
            notification.SetEwcCodes(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Ewc))
            });

            notification.SetHCodes(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.H))
            });

            notification.SetYCodes(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Y))
            });

            notification.SetUnClasses(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.Un))
            });

            notification.SetUnNumbers(new[]
            {
                WasteCodeInfo.CreateWasteCodeInfo(wasteCodes.First(wc => wc.CodeType == CodeType.UnNumber))
            });

            notification.SetCustomsCode(
                WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.CustomsCode,
                    "olives"));
            notification.SetImportCode(
                WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.ImportCode,
                    "cardboard boxes"));
            notification.SetExportCode(
                WasteCodeInfo.CreateCustomWasteCodeInfo(CodeType.ExportCode,
                    "gravel"));
        }
    }
}