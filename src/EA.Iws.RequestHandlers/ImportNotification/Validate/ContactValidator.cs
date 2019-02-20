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
                .WithLocalizedMessage(() => ContactValidatorResources.ContactNameNotEmpty);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithLocalizedMessage(() => ContactValidatorResources.EmailNotEmpty)
                .EmailAddress()
                .WithLocalizedMessage(() => ContactValidatorResources.EmailNotValid);

            RuleFor(x => x.Telephone)
                .NotEmpty()
                .WithLocalizedMessage(() => ContactValidatorResources.TelephoneNotEmpty);

            RuleFor(x => x.TelephonePrefix)
                .NotEmpty()
                .WithLocalizedMessage(() => ContactValidatorResources.TelephonePrefixNotEmpty);
        }
    }
}