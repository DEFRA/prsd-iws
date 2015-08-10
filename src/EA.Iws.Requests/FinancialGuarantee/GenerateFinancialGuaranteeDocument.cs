namespace EA.Iws.Requests.FinancialGuarantee
{
    using Core.Notification;
    using Prsd.Core.Mediator;

    public class GenerateFinancialGuaranteeDocument : IRequest<byte[]>
    {
        public CompetentAuthority CompetentAuthority { get; private set; }

        public GenerateFinancialGuaranteeDocument(CompetentAuthority competentAuthority)
        {
            CompetentAuthority = competentAuthority;
        }
    }
}
