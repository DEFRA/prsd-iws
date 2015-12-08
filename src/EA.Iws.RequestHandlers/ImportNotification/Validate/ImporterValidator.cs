namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ImporterValidator : AbstractValidator<Importer>
    {
        public ImporterValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => ImporterValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithLocalizedMessage(() => ImporterValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(ImporterAddressIsNotEmpty);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => ImporterValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithLocalizedMessage(() => ImporterValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(ImporterContactIsNotEmpty);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithLocalizedMessage(() => ImporterValidatorResources.BusinessNameNotEmpty);

            RuleFor(x => x.Type)
                .NotNull()
                .WithLocalizedMessage(() => ImporterValidatorResources.BusinessTypeNotEmpty);
        }

        private bool ImporterContactIsNotEmpty(Importer importer)
        {
            return importer != null && BeNonEmptyContact(importer.Contact);
        }

        private bool BeNonEmptyContact(Contact contact)
        {
            return contact != null && !contact.IsEmpty;
        }

        private bool ImporterAddressIsNotEmpty(Importer importer)
        {
            return importer != null && BeNonEmptyAddress(importer.Address);
        }

        private bool BeNonEmptyAddress(Address address)
        {
            return address != null && !address.IsEmpty;
        }
    }
}