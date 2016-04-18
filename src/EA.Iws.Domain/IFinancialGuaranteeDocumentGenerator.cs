namespace EA.Iws.Domain
{
    public interface IFinancialGuaranteeDocumentGenerator
    {
        byte[] GenerateFinancialGuaranteeDocument(Core.Notification.UKCompetentAuthority competentAuthority);
        byte[] GenerateBankGuaranteeDocument();
        byte[] GenerateParentCompanyTemplate();
    }
}