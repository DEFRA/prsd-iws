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
                .WithMessage(x => ExporterValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithMessage(x => ExporterValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(ExporterAddressIsNotEmpty);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(x => ExporterValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithMessage(x => ExporterValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(ExporterContactIsNotEmpty);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithMessage(x => ExporterValidatorResources.BusinessNameNotEmpty);
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