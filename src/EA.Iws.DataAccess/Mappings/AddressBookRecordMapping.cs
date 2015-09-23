namespace EA.Iws.DataAccess.Mappings
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
