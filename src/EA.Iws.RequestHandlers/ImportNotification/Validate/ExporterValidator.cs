namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ExporterValidator : AbstractValidator<Exporter>
    {
        public ExporterValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address).NotEmpty().SetValidator(addressValidator);
            RuleFor(x => x.Contact).NotEmpty().SetValidator(contactValidator);
            RuleFor(x => x.BusinessName).NotEmpty();
        }
    }
}