namespace EA.Iws.TestHelpers.DomainFakes
{
    using System;
    using Domain;
    using Helpers;

    public class TestableCountry : Country
    {
        public new Guid Id
        {
            get { return base.Id; }
            set { base.Id = value; }
        }

        public new string Name
        {
            get { return base.Name; }
            set { ObjectInstantiator<Country>.SetProperty(x => x.Name, value, this); }
        }

        public new string IsoAlpha2Code
        {
            get { return base.IsoAlpha2Code; }
            set { ObjectInstantiator<Country>.SetProperty(x => x.IsoAlpha2Code, value, this); }
        }

        public new bool IsEuropeanUnionMember
        {
            get { return base.IsEuropeanUnionMember; }
            set { ObjectInstantiator<Country>.SetProperty(x => x.IsEuropeanUnionMember, value, this); }
        }

        public static Country UnitedKingdom
        {
            get
            {
                return new TestableCountry
                {
                    Id = new Guid("938F3051-4D25-4995-A22E-A7562122C85E"),
                    IsEuropeanUnionMember = true,
                    IsoAlpha2Code = "GB",
                    Name = "United Kingdom"
                };
            }
        }

        public static Country France
        {
            get
            {
                return new TestableCountry
                {
                    Id = new Guid("41B93E0B-4DC2-4DF1-B751-C5123B55BB26"),
                    Name = "France",
                    IsoAlpha2Code = "FR",
                    IsEuropeanUnionMember = true
                };
            }
        }

        public static Country Switzerland
        {
            get
            {
                return new TestableCountry
                {
                    Id = new Guid("642F4ADD-A79E-4D35-BD92-0539734D00A6"),
                    Name = "Switzerland",
                    IsEuropeanUnionMember = false,
                    IsoAlpha2Code = "CH"
                };
            }
        }
    }
}
