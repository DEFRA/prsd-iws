namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using Domain;
    using UKCompetentAuthority = Core.Notification.UKCompetentAuthority;

    public class FinancialGuaranteeDocumentGenerator : IFinancialGuaranteeDocumentGenerator
    {
        public byte[] GenerateFinancialGuaranteeDocument(UKCompetentAuthority competentAuthority)
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared(GetFinancialGuaranteeDocumentName(competentAuthority)))
            {
                return memoryStream.ToArray();
            }
        }

        public byte[] GenerateBankGuaranteeDocument()
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("SEPABankGuaranteeTemplate.doc"))
            {
                return memoryStream.ToArray();
            }
        }

        public byte[] GenerateParentCompanyTemplate()
        {
            using (var memoryStream = DocumentHelper.ReadDocumentStreamShared("SEPAParentCompanyTemplate.doc"))
            {
                return memoryStream.ToArray();
            }
        }

        private string GetFinancialGuaranteeDocumentName(UKCompetentAuthority competentAuthority)
        {
            string filename;

            switch (competentAuthority)
            {
                case UKCompetentAuthority.NorthernIreland:
                    filename = "NIEAFinancialGuaranteeForm.pdf";
                    break;
                case UKCompetentAuthority.Wales:
                    filename = "NRWFinancialGuaranteeForm.pdf";
                    break;
                default:
                    filename = "EAFinancialGuaranteeForm.pdf";
                    break;
            }

            return filename;
        }
    }
}
