namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ProducerValidator : AbstractValidator<Producer>
    {
        public ProducerValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => ProducerValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithLocalizedMessage(() => ProducerValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(ProducerAddressIsNotEmpty);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithLocalizedMessage(() => ProducerValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithLocalizedMessage(() => ProducerValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(ProducerContactIsNotEmpty);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithLocalizedMessage(() => ProducerValidatorResources.BusinessNameNotEmpty);
        }

        private bool ProducerContactIsNotEmpty(Producer producer)
        {
            return producer != null && BeNonEmptyContact(producer.Contact);
        }

        private bool BeNonEmptyContact(Contact contact)
        {
            return contact != null && !contact.IsEmpty;
        }

        private bool BeNonEmptyAddress(Address address)
        {
            return address != null && !address.IsEmpty;
        }

        private bool ProducerAddressIsNotEmpty(Producer producer)
        {
            return producer != null && BeNonEmptyAddress(producer.Address);
        }
    }
}