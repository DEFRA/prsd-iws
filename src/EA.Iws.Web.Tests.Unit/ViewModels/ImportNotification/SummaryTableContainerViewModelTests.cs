namespace EA.Iws.Web.Tests.Unit.ViewModels.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Areas.ImportNotification.ViewModels.Home;
    using Core.ImportNotification.Summary;
    using Core.ImportNotificationAssessment;
    using Core.Shared;
    using TestHelpers;
    using Xunit;

    public class SummaryTableContainerViewModelTests
    {
        private const string Ewc1 = "01 04 09";
        private const string Ewc2 = "01 05 10*";
        private const string Ewc3 = "02 01 01";

        private const string Y1 = "Y1";
        private const string Y2 = "Y4";
        private const string Y3 = "Y10";

        private const string H1 = "H1";
        private const string H2 = "H2";
        private const string H3 = "H10";

        private const string Un1 = "2";
        private const string Un2 = "5";

        [Fact]
        public void NotificationReceived_ShowChangeLinks()
        {
            var model = CreateModel(ImportNotificationStatus.NotificationReceived, true, true);

            Assert.True(model.ShowChangeLinks);
        }

        [Fact]
        public void NotificationConsented_ShowChangeNumberOfShipmentsLink()
        {
            var model = CreateModel(ImportNotificationStatus.Consented, true, true);

            Assert.True(model.ShowChangeNumberOfShipmentsLink);
        }

        [Fact]
        public void Notification_Not_New_Or_Received_ShowChangeEntryExitPointLink()
        {
            var model = CreateModel(ImportNotificationStatus.InAssessment, true, true);

            Assert.True(model.ShowChangeEntryExitPointLink);
        }

        [Fact]
        public void EwcCodesAreInNumericalOrder()
        {
            var model = CreateModel(ImportNotificationStatus.NotificationReceived, true, true);

            var result = model.Details.WasteType.EwcCodes.WasteCodes;
            var expected = CreateEwcCodes().OrderBy(w => w.Name).ToList();

            Assert.True(result.SequenceEqual(expected, new WasteCodeComparer()));
        }

        [Fact]
        public void YCodesAreInNumericalOrder()
        {
            var model = CreateModel(ImportNotificationStatus.NotificationReceived, true, true);

            var result = model.Details.WasteType.YCodes.WasteCodes;
            var expected = CreateYCodes().OrderBy(w => Regex.Match(w.Name, @"(\D+)").Value).ThenBy(w =>
            {
                int val;
                int.TryParse(Regex.Match(w.Name, @"(\d+)").Value, out val);
                return val;
            });

            Assert.True(result.SequenceEqual(expected, new WasteCodeComparer()));
        }

        [Fact]
        public void HCodesAreInNumbericalOrder()
        {
            var model = CreateModel(ImportNotificationStatus.NotificationReceived, true, true);

            var result = model.Details.WasteType.HCodes.WasteCodes;
            var expected = CreateHCodes().OrderBy(w => Regex.Match(w.Name, @"(\D+)").Value).ThenBy(w =>
            {
                int val;
                int.TryParse(Regex.Match(w.Name, @"(\d+)").Value, out val);
                return val;
            });

            Assert.True(result.SequenceEqual(expected, new WasteCodeComparer()));
        }

        [Fact]
        public void UnClassesAreInNumericalOrder()
        {
            var model = CreateModel(ImportNotificationStatus.NotificationReceived, true, true);

            var result = model.Details.WasteType.UnClasses.WasteCodes;
            var expected = CreateUnClasses().OrderBy(w => w.Name).ToList();

            Assert.True(result.SequenceEqual(expected, new WasteCodeComparer()));
        }

        private SummaryTableContainerViewModel CreateModel(ImportNotificationStatus status, bool canChangeNumberOfShipments, bool canChangeEntryExitPoint)
        {
            return new SummaryTableContainerViewModel(CreateImportNotifiationSummary(status), canChangeNumberOfShipments, canChangeEntryExitPoint);
        }

        private ImportNotificationSummary CreateImportNotifiationSummary(ImportNotificationStatus status)
        {
            var wasteType = new WasteType()
            {
                EwcCodes = new WasteCodeSelection() { WasteCodes = CreateEwcCodes() },
                YCodes = new WasteCodeSelection() { WasteCodes = CreateYCodes() },
                HCodes = new WasteCodeSelection() { WasteCodes = CreateHCodes() },
                UnClasses = new WasteCodeSelection() { WasteCodes = CreateUnClasses() }
            };

            var summary = new ImportNotificationSummary()
            {
                Id = Guid.NewGuid(),
                Type = NotificationType.Recovery,
                Status = status,
                Number = "GB123",
                WasteType = wasteType
            };

            return summary;
        }

        private IList<WasteCode> CreateEwcCodes()
        {
            return new List<WasteCode>()
            {
                new WasteCode() { Name = Ewc2, Description = "wastes from mineral metalliferous excavation" },
                new WasteCode() { Name = Ewc1, Description = "waste sand and clays" },
                new WasteCode() { Name = Ewc3, Description = "sludges from washing and cleaning" }
            };
        }

        private IList<WasteCode> CreateYCodes()
        {
            return new List<WasteCode>()
            {
                new WasteCode() { Name = Y3, Description = "Metal carbonyls" },
                new WasteCode() { Name = Y2, Description = "Copper compounds" },
                new WasteCode() { Name = Y1, Description = "Zinc compounds" }
            };
        }

        private IList<WasteCode> CreateHCodes()
        {
            return new List<WasteCode>()
            {
                new WasteCode() { Name = H1, Description = "Explosive" },
                new WasteCode() { Name = H3, Description = "Liberation of toxic gases in contact with air or water" },
                new WasteCode() { Name = H2, Description = "Ecotoxic" }
            };
        }

        private IList<WasteCode> CreateUnClasses()
        {
            return new List<WasteCode>()
            {
                new WasteCode() { Name = Un1, Description = "Explosives" },
                new WasteCode() { Name = Un2, Description = "Flammable solids" }
            };
        }
    }
}
