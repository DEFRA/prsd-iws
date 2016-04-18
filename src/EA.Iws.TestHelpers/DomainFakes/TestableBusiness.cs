namespace EA.Iws.TestHelpers.DomainFakes
{
    using Core.Shared;
    using Domain.NotificationApplication;
    using Helpers;

    public class TestableBusiness : Business
    {
        public new string Name
        {
            get { return base.Name; }
            set { ObjectInstantiator<Business>.SetProperty(x => x.Name, value, this); }
        }

        public new BusinessType Type
        {
            get { return base.Type; }
            set { ObjectInstantiator<Business>.SetProperty(x => x.Type, value, this); }
        }

        public new string RegistrationNumber
        {
            get { return base.RegistrationNumber; }
            set { ObjectInstantiator<Business>.SetProperty(x => x.RegistrationNumber, value, this); }
        }

        public new string AdditionalRegistrationNumber
        {
            get { return base.AdditionalRegistrationNumber; }
            set { ObjectInstantiator<Business>.SetProperty(x => x.AdditionalRegistrationNumber, value, this); }
        }

        public new string OtherDescription
        {
            get { return base.OtherDescription; }
            set { ObjectInstantiator<Business>.SetProperty(x => x.OtherDescription, value, this); }
        }

        public static Business WasteSolutions
        {
            get
            {
                return new TestableBusiness
                {
                    Name = "Waste Solutions Ltd",
                    Type = BusinessType.SoleTrader,
                    RegistrationNumber = "46541531"
                };
            }
        }

        public static Business CSharpGarbageCollector
        {
            get
            {
                return new TestableBusiness
                {
                    Name = "Garbage Collection Engineering",
                    Type = BusinessType.LimitedCompany,
                    RegistrationNumber = "556566"
                };
            }
        }

        public static Business LargeObjectHeap
        {
            get
            {
                return new TestableBusiness
                {
                    Name = "Environmental Protection Solutions Delivered",
                    Type = BusinessType.Other,
                    OtherDescription = "Charity",
                    RegistrationNumber = "5643553",
                    AdditionalRegistrationNumber = "745744"
                };
            }
        }
    }
}