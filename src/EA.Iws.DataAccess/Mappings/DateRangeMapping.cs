namespace EA.Iws.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain;

    internal class DateRangeMapping : ComplexTypeConfiguration<DateRange>
    {
        public DateRangeMapping()
        {
            Property(x => x.From).HasColumnName("From").IsRequired();
            Property(x => x.To).HasColumnName("To").IsRequired();

            Ignore(x => x.Days);
        }
    }
}
