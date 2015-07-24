namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.FinancialGuarantee
{
    using DataAccess;
    using FakeItEasy;

    public class SetFinancialGuaranteeDatesHandlerTests
    {
        private readonly IwsContext context;

        public SetFinancialGuaranteeDatesHandlerTests()
        {
            context = A.Fake<IwsContext>();
        }
    }
}
