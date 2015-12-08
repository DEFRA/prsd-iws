namespace EA.Iws.DataAccess.Mappings.Exports
{
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using Domain.AddressBook;
    using Prsd.Core.Helpers;

    public class AddressBookMapping : EntityTypeConfiguration<AddressBook>
    {
        public AddressBookMapping()
        {
            ToTable("AddressBook", "Person");

            HasMany(
                ExpressionHelper.GetPrivatePropertyExpression<AddressBook, ICollection<AddressBookRecord>>(
                    "AddressCollection"))
                .WithRequired()
                .Map(m => m.MapKey("AddressBookId"));
        }
    }
}
