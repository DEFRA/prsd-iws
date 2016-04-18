namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain;
    using Domain.TransportRoute;

    public class TestableStateOfExport : StateOfExport
    {
        public new Country Country
        {
            get { return base.Country; }
            set { base.Country = value; }
        }

        public new CompetentAuthority CompetentAuthority
        {
            get { return base.CompetentAuthority; }
            set { base.CompetentAuthority = value; }
        }

        public new EntryOrExitPoint ExitPoint
        {
            get { return base.ExitPoint; }
            set { base.ExitPoint = value; }
        }
    }
}
