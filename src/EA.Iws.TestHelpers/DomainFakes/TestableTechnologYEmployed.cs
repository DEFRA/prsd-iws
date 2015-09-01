namespace EA.Iws.TestHelpers.DomainFakes
{
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableTechnologyEmployed : TechnologyEmployed
    {
        public new bool AnnexProvided 
        {
            get { return base.AnnexProvided; }
            set { ObjectInstantiator<TechnologyEmployed>.SetProperty(x => x.AnnexProvided, value, this); }
        }

        public new string Details
        {
            get { return base.Details; }
            set { ObjectInstantiator<TechnologyEmployed>.SetProperty(x => x.Details, value, this); }
        }

        public string FurtherDetails
        {
            get { return base.FurtherDetails; }
            set { ObjectInstantiator<TechnologyEmployed>.SetProperty(x => x.FurtherDetails, value, this); }
        }
    }
}
