namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    public class ProducerValidator : AbstractValidator<Producer>
    {
        [System.Obsolete]
        public ProducerValidator(IValidator<Address> addressValidator,
            IValidator<Contact> contactValidator)
        {
            RuleFor(x => x.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(x => ProducerValidatorResources.AddressNotEmpty)
                .Must(BeNonEmptyAddress)
                .WithMessage(x => ProducerValidatorResources.AddressNotEmpty);

            RuleFor(x => x.Address)
                .SetValidator(addressValidator)
                .When(ProducerAddressIsNotEmpty);

            RuleFor(x => x.Contact)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage(x => ProducerValidatorResources.ContactNotEmpty)
                .Must(BeNonEmptyContact)
                .WithMessage(x => ProducerValidatorResources.ContactNotEmpty);

            RuleFor(x => x.Contact)
                .SetValidator(contactValidator)
                .When(ProducerContactIsNotEmpty);

            RuleFor(x => x.BusinessName)
                .NotEmpty()
                .WithMessage(x => ProducerValidatorResources.BusinessNameNotEmpty);
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