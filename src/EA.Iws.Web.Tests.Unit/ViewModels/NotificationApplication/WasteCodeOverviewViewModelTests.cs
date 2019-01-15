namespace EA.Iws.Web.Tests.Unit.ViewModels.NotificationApplication
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Areas.NotificationApplication.ViewModels.NotificationApplication;
    using Core.Notification;
    using Core.Notification.Overview;
    using Core.WasteCodes;
    using TestHelpers;
    using Xunit;

    public class WasteCodeOverviewViewModelTests
    {
        [Fact]
        public void EwcCodesAreInNumericalOrder()
        {
            var model = CreateModel();

            var result = model.EwcCodes;
            var expected = CreateEwcCodes().OrderBy(w => w.Code).ToArray();

            Assert.True(result.SequenceEqual(expected, new WasteCodeDataComparer()));
        }

        [Fact]
        public void YCodesAreInNumericalOrder()
        {
            var model = CreateModel();

            var result = model.YCodes;
            var expected = CreateYCodes().OrderBy(w => Regex.Match(w.Code, @"(\D+)").Value).ThenBy(w =>
            {
                int val;
                int.TryParse(Regex.Match(w.Code, @"(\d+)").Value, out val);
                return val;
            }).ToArray();

            Assert.True(result.SequenceEqual(expected, new WasteCodeDataComparer()));
        }

        [Fact]
        public void HCodesAreInNumericalOrder()
        {
            var model = CreateModel();

            var result = model.HCodes;
            var expected = CreateHCodes().OrderBy(w => Regex.Match(w.Code, @"(\D+)").Value).ThenBy(w =>
            {
                int val;
                int.TryParse(Regex.Match(w.Code, @"(\d+)").Value, out val);
                return val;
            }).ToArray();

            Assert.True(result.SequenceEqual(expected, new WasteCodeDataComparer()));
        }

        [Fact]
        public void UnClassAreInNumericalOrder()
        {
            var model = CreateModel();

            var result = model.UnClass;
            var expected = CreateUnClasses().OrderBy(w => w.Code).ToArray();

            Assert.True(result.SequenceEqual(expected, new WasteCodeDataComparer()));
        }

        [Fact]
        public void UnNumberAreInNumericalOrder()
        {
            var model = CreateModel();

            var result = model.UnNumber;
            var expected = CreateUnNumber().OrderBy(w => w.Code).ToArray();

            Assert.True(result.SequenceEqual(expected, new WasteCodeDataComparer()));
        }

        private static WasteCodeOverviewViewModel CreateModel()
        {
            return new WasteCodeOverviewViewModel(CreateOverviewInfo(), CreateCompletionProgress());
        }

        private static WasteCodesOverviewInfo CreateOverviewInfo()
        {
            return new WasteCodesOverviewInfo()
            {
                NotificationId = Guid.NewGuid(),
                EwcCodes = CreateEwcCodes(),
                YCodes = CreateYCodes(),
                HCodes = CreateHCodes(),
                UnClass = CreateUnClasses(),
                UnNumber = CreateUnNumber()
            };
        }

        private static NotificationApplicationCompletionProgress CreateCompletionProgress()
        {
            return new NotificationApplicationCompletionProgress()
            {
                HasBaselOecdCode = true,
                HasEwcCodes = true,
                HasYCodes = true,
                HasHCodes = true,
                HasUnClasses = true,
                HasUnNumbers = true,
                HasOtherCodes = true
            };
        }

        private static WasteCodeData[] CreateEwcCodes()
        {
            return new[]
            {
                new WasteCodeData() { Code = "20 01 37*" },
                new WasteCodeData() { Code = "06 13 02*" },
                new WasteCodeData() { Code = "02 04 99" }
            };
        }

        private static WasteCodeData[] CreateYCodes()
        {
            return new[]
            {
                new WasteCodeData() { Code = "Y11" },
                new WasteCodeData() { Code = "Y1" },
                new WasteCodeData() { Code = "Y2" }
            };
        }

        private static WasteCodeData[] CreateHCodes()
        {
            return new[]
            {
                new WasteCodeData() { Code = "H12" },
                new WasteCodeData() { Code = "HP21" },
                new WasteCodeData() { Code = "H1" },
                new WasteCodeData() { Code = "HP2" },
                new WasteCodeData() { Code = "H4" }
            };
        }

        private static WasteCodeData[] CreateUnClasses()
        {
            return new[]
            {
                new WasteCodeData() { Code = "4.1" },
                new WasteCodeData() { Code = "7" },
                new WasteCodeData() { Code = "2" }
            };
        }

        private static WasteCodeData[] CreateUnNumber()
        {
            return new[]
            {
                new WasteCodeData() { Code = "UN 1463" },
                new WasteCodeData() { Code = "UN 1011" },
                new WasteCodeData() { Code = "UN 0289" }
            };
        }
    }
}
