namespace EA.Iws.DocumentGeneration.DocumentGenerator
{
    using Domain;
    using CompetentAuthority = Core.Notification.CompetentAuthority;

    public class FinancialGuaranteeDocumentGenerator : IFinancialGuaranteeDocumentGenerator
    {
        public byte[] GenerateFinancialGuaranteeDocument(CompetentAuthority competentAuthority)
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

        private string GetFinancialGuaranteeDocumentName(CompetentAuthority competentAuthority)
        {
            string filename;

            switch (competentAuthority)
            {
                case CompetentAuthority.NorthernIreland:
                    filename = "NIEAFinancialGuaranteeForm.pdf";
                    break;
                case CompetentAuthority.Wales:
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
