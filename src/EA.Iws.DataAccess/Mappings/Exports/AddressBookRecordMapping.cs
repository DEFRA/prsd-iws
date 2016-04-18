namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.AddressBook;

    public class AddressBookRecordMapping : EntityTypeConfiguration<AddressBookRecord>
    {
        public AddressBookRecordMapping()
        {
            ToTable("AddressBookRecord", "Person");
        }
    }
}
