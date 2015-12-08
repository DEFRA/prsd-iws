namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ExporterValidator : AbstractValidator<Exporter>
    {
        public ExporterValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => ExporterValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithLocalizedMessage(() => ExporterValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(ExporterAddressIsNotEmpty);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => ExporterValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithLocalizedMessage(() => ExporterValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(ExporterContactIsNotEmpty);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithLocalizedMessage(() => ExporterValidatorResources.BusinessNameNotEmpty);
        }

        private bool ExporterContactIsNotEmpty(Exporter exporter)
        {
            return exporter != null && BeNonEmptyContact(exporter.Contact);
        }

        private bool BeNonEmptyContact(Contact contact)
        {
            return contact != null && !contact.IsEmpty;
        }

        private bool ExporterAddressIsNotEmpty(Exporter exporter)
        {
            return exporter != null && BeNonEmptyAddress(exporter.Address);
        }

        private bool BeNonEmptyAddress(Address address)
        {
            return address != null && !address.IsEmpty;
        }
    }
}