namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ProducerValidator : AbstractValidator<Producer>
    {
        public ProducerValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address).SetValidator(addressValidator);
            RuleFor(x => x.Contact).SetValidator(contactValidator);
            RuleFor(x => x.BusinessName).NotEmpty();
        }
    }
}