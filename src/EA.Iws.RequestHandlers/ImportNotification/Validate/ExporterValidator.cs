namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ExporterValidator : AbstractValidator<Exporter>
    {
        public ExporterValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address).SetValidator(addressValidator);
            RuleFor(x => x.Contact).SetValidator(contactValidator);
            RuleFor(x => x.BusinessName).NotEmpty();
        }
    }
}