namespace EA.Iws.DataAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class BankHolidayMapping : EntityTypeConfiguration<BankHoliday>
    {
        public BankHolidayMapping()
        {
            ToTable("BankHoliday", "Lookup");
            
            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
