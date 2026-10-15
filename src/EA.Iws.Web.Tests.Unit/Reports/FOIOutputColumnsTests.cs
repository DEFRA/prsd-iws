namespace EA.Iws.Web.Tests.Unit.Reports
{
    using System;
    using System.Linq;
    using EA.Iws.Core.Admin.Reports;
    using EA.Iws.Core.Reports.FOI;
    using Xunit;

    public class FOIOutputColumnsTests
    {
        [Fact]
        public void EveryFreedomOfInformationDataProperty_HasCorrespondingEnumMember()
        {
            var propertyNames = typeof(FreedomOfInformationData).GetProperties().Select(p => p.Name).ToList();
            var enumNames = Enum.GetNames(typeof(FOIOutputColumns)).ToList();

            var missing = propertyNames.Except(enumNames).ToList();

            Assert.True(missing.Count == 0,
                $"The following FreedomOfInformationData properties have no corresponding checkbox in FOIOutputColumns: {string.Join(", ", missing)}");
        }

        [Fact]
        public void EveryEnumMember_HasCorrespondingFreedomOfInformationDataProperty()
        {
            var propertyNames = typeof(FreedomOfInformationData).GetProperties().Select(p => p.Name).ToList();
            var enumNames = Enum.GetNames(typeof(FOIOutputColumns)).ToList();

            var orphaned = enumNames.Except(propertyNames).ToList();

            Assert.True(orphaned.Count == 0,
                $"The following FOIOutputColumns members do not match any FreedomOfInformationData property: {string.Join(", ", orphaned)}");
        }
    }
}