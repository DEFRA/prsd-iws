namespace EA.Iws.Domain
{
    public interface IFinancialGuaranteeDocumentGenerator
    {
        byte[] GenerateFinancialGuaranteeDocument(Core.Notification.CompetentAuthority competentAuthority);
        byte[] GenerateBankGuaranteeDocument();
        byte[] GenerateParentCompanyTemplate();
    }
}