namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using Xunit;

    public class StateOfExportTests
    {
        private readonly Guid anyGuid = new Guid("DE6CA75B-41B3-4781-ABE9-6BF09C9FC639");

        [Fact]
        public void CanCreateStateOfExport()
        {
            var stateOfImport = new StateOfExport(anyGuid, anyGuid, anyGuid);

            Assert.IsType<StateOfExport>(stateOfImport);
        }
    }
}