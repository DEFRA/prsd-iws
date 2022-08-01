namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ContactValidator : AbstractValidator<Contact>
    {
        public ContactValidator()
        {
            RuleFor(x => x.ContactName)
                .NotEmpty()
                .WithMessage(x => ContactValidatorResources.ContactNameNotEmpty);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(x => ContactValidatorResources.EmailNotEmpty)
                .EmailAddress()
                .WithMessage(x => ContactValidatorResources.EmailNotValid);

            RuleFor(x => x.Telephone)
                .NotEmpty()
                .WithMessage(x => ContactValidatorResources.TelephoneNotEmpty);

            RuleFor(x => x.TelephonePrefix)
                .NotEmpty()
                .WithMessage(x => ContactValidatorResources.TelephonePrefixNotEmpty);
        }
    }
}