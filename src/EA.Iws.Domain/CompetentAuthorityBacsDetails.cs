namespace EA.Iws.Domain
{
    public class CompetentAuthorityBacsDetails
    {
        public string AccountName { get; protected set; }

        public string Bank { get; protected set; }

        public string BankAddress { get; protected set; }

        public string SortCode { get; protected set; }

        public string AccountNumber { get; protected set; }

        public string Iban { get; protected set; }

        public string SwiftBic { get; protected set; }

        public string Email { get; protected set; }

        public string Fax { get; protected set; }

        protected CompetentAuthorityBacsDetails()
        {
        }
    }
}