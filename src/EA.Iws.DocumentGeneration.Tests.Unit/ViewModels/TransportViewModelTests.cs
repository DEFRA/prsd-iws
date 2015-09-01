namespace EA.Iws.DocumentGeneration.Tests.Unit.ViewModels
{
    using DocumentGeneration.ViewModels;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class TransportViewModelTests
    {
        private readonly TestableNotificationApplication notification = new TestableNotificationApplication
        {
            StateOfExport = new TestableStateOfExport
            {
                CompetentAuthority = TestableCompetentAuthority.EnvironmentAgency,
                Country = TestableCountry.UnitedKingdom,
                ExitPoint = TestableEntryOrExitPoint.Dover
            },
            StateOfImport = new TestableStateOfImport
            {
                CompetentAuthority = TestableCompetentAuthority.SwissAuthority,
                Country = TestableCountry.Switzerland,
                EntryPoint = TestableEntryOrExitPoint.Basel
            }
        };

        [Fact]
        public void ExportCountry()
        {
            var model = new TransportViewModel(notification);

            Assert.Equal(TestableCountry.UnitedKingdom.Name, model.ExportCountry);
        }

        [Fact]
        public void ExportCompetentAuthority()
        {
            var model = new TransportViewModel(notification);

            Assert.Equal(TestableCompetentAuthority.EnvironmentAgency.Code, model.ExportCompetentAuthority);
        }

        [Fact]
        public void ExportExitPoint()
        {
            var model = new TransportViewModel(notification);

            Assert.Equal(TestableEntryOrExitPoint.Dover.Name, model.ExitPoint);
        }

        [Fact]
        public void ImportCountry()
        {
            var model = new TransportViewModel(notification);

            Assert.Equal(TestableCountry.Switzerland.Name, model.ImportCountry);
        }

        [Fact]
        public void ImportCompetentAuthority()
        {
            var model = new TransportViewModel(notification);

            Assert.Equal(TestableCompetentAuthority.SwissAuthority.Code, model.ImportCompetentAuthority);
        }

        [Fact]
        public void ImportEntryPoint()
        {
            var model = new TransportViewModel(notification);

            Assert.Equal(TestableEntryOrExitPoint.Basel.Name, model.EntryPoint);
        }
    }
}
