namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class CompetentAuthorityBacsDetailsMapping : ComplexTypeConfiguration<CompetentAuthorityBacsDetails>
    {
        private const string ColumnPrefix = "Bacs";

        public CompetentAuthorityBacsDetailsMapping()
        {
            Property(x => x.AccountName).HasColumnName(ColumnPrefix + "AccountName").IsRequired().HasMaxLength(4000);
            Property(x => x.Bank).HasColumnName(ColumnPrefix + "Bank").IsRequired().HasMaxLength(2048);
            Property(x => x.BankAddress).HasColumnName(ColumnPrefix + "BankAddress");
            Property(x => x.SortCode).HasColumnName(ColumnPrefix + "SortCode").IsRequired().HasMaxLength(8).IsFixedLength();
            Property(x => x.AccountNumber).HasColumnName(ColumnPrefix + "AccountNumber").HasMaxLength(256).IsRequired();
            Property(x => x.Iban).HasColumnName(ColumnPrefix + "Iban").HasMaxLength(256);
            Property(x => x.SwiftBic).HasColumnName(ColumnPrefix + "SwiftBic").IsRequired().HasMaxLength(256);
            Property(x => x.Email).HasColumnName(ColumnPrefix + "Email").HasMaxLength(256);
            Property(x => x.Fax).HasColumnName(ColumnPrefix + "Fax").HasMaxLength(128);
        }
    }
}
