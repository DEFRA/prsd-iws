namespace EA.Iws.Domain.Tests.Unit.ImportNotification
{
    using System;
    using Domain.ImportNotification;
    using Xunit;

    public class StateOfImportTests
    {
        private readonly Guid anyGuid = new Guid("DE6CA75B-41B3-4781-ABE9-6BF09C9FC639");

        [Fact]
        public void CanCreateStateOfImport()
        {
            var stateOfImport = new StateOfImport(anyGuid, anyGuid);

            Assert.IsType<StateOfImport>(stateOfImport);
        }
    }
}