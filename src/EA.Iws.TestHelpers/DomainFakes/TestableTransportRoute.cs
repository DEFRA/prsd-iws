namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Domain.TransportRoute;
    using Helpers;

    public class TestableTransportRoute : TransportRoute
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { ObjectInstantiator<TransportRoute>.SetProperty(x => x.Id, value, this); }
        }

        public new StateOfExport StateOfExport
        {
            get { return base.StateOfExport; }
            set { ObjectInstantiator<TransportRoute>.SetProperty(x => x.StateOfExport, value, this); }
        }

        public new StateOfImport StateOfImport
        {
            get { return base.StateOfImport; }
            set { ObjectInstantiator<TransportRoute>.SetProperty(x => x.StateOfImport, value, this); }
        }

        public new IList<TransitState> TransitStates
        {
            get { return base.TransitStates.ToList(); }
            set { TransitStatesCollection = value; }
        }
    }
}