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
                .WithMessage(x => ImporterValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithMessage(x => ImporterValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(ImporterAddressIsNotEmpty);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(x => ImporterValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithMessage(x => ImporterValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(ImporterContactIsNotEmpty);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithMessage(x => ImporterValidatorResources.BusinessNameNotEmpty);

            RuleFor(x => x.Type)
                .NotNull()
                .WithMessage(x => ImporterValidatorResources.BusinessTypeNotEmpty);
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