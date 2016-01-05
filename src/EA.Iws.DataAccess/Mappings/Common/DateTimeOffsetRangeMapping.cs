namespace EA.Iws.DataAccess.Mappings.Common
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class DateTimeOffsetRangeMapping : ComplexTypeConfiguration<DateTimeOffsetRange>
    {
        public DateTimeOffsetRangeMapping()
        {
            Property(x => x.From).HasColumnName("From").IsRequired();
            Property(x => x.To).HasColumnName("To").IsRequired();

            Ignore(x => x.Days);
        }
    }
}
