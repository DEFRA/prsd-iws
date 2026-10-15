namespace EA.Iws.Web.Tests.Unit.Reports
{
    using System;
    using System.Linq;
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Reports;
    using Xunit;

    public class ShipmentReportOutputColumnsTests
    {
        [Fact]
        public void EveryShipmentDataProperty_HasCorrespondingEnumMember()
        {
            var propertyNames = typeof(ShipmentData).GetProperties().Select(p => p.Name).ToList();
            var enumNames = Enum.GetNames(typeof(ShipmentReportOutputColumns)).ToList();

            var missing = propertyNames.Except(enumNames).ToList();

            Assert.True(missing.Count == 0,
                $"The following ShipmentData properties have no corresponding checkbox in ShipmentReportOutputColumns: {string.Join(", ", missing)}");
        }

        [Fact]
        public void EveryEnumMember_HasCorrespondingShipmentDataProperty()
        {
            var propertyNames = typeof(ShipmentData).GetProperties().Select(p => p.Name).ToList();
            var enumNames = Enum.GetNames(typeof(ShipmentReportOutputColumns)).ToList();

            var orphaned = enumNames.Except(propertyNames).ToList();

            Assert.True(orphaned.Count == 0,
                $"The following ShipmentReportOutputColumns members do not match any ShipmentData property: {string.Join(", ", orphaned)}");
        }
    }
}