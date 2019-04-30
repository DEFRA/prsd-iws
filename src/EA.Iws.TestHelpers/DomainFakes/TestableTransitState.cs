namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;
    using Domain.TransportRoute;
    using Helpers;

    public class TestableTransitState : TransitState
    {
        public TestableTransitState()
        {
        }
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<TransitState>.SetProperty(x => x.Id, value, this); }
        }

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

        public new EntryOrExitPoint EntryPoint
        {
            get { return base.EntryPoint; }
            set { base.EntryPoint = value; }
        }

        public new EntryOrExitPoint ExitPoint
        {
            get { return base.ExitPoint; }
            set { base.ExitPoint = value; }
        }
    }
}
