namespace EA.Iws.TestHelpers.Helpers
{
    using Domain;

    public class ComplexTypeFactory
    {
        private const string AnyString = "test";
        public static TComplexType Create<TComplexType>(string name = AnyString) where TComplexType : class 
        {
            var argumentType = typeof(TComplexType);

            if (argumentType == typeof(Address))
            {
                return new Address(AnyString, AnyString, AnyString, AnyString, AnyString, AnyString) as TComplexType;
            }

            if (argumentType == typeof(Business))
            {
                return Business.CreateBusiness(name, BusinessType.SoleTrader, AnyString, null) as TComplexType; 
            }

            if (argumentType == typeof(Contact))
            {
                return new Contact(name, AnyString, AnyString, AnyString, AnyString) as TComplexType;
            }

            if (argumentType == typeof(ProducerBusiness))
            {
                return ProducerBusiness.CreateProducerBusiness(name, BusinessType.SoleTrader, AnyString, null) as TComplexType;
            }

            return null;
        }
    }
}
