namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Core.OperationCodes;
    using FluentValidation;

    internal class WasteOperationValidator : AbstractValidator<WasteOperation>
    {
        public WasteOperationValidator()
        {
            RuleFor(x => x.OperationCodes)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithLocalizedMessage(() => WasteOperationValidatorResources.OperationCodesNotEmpty)
                .Must(BeOfSameType)
                .WithLocalizedMessage(() => WasteOperationValidatorResources.OperationCodesOfSameType);

            RuleFor(x => x.TechnologyEmployed)
                .Length(0, 70)
                .WithLocalizedMessage(() => WasteOperationValidatorResources.TechnologyEmployedMaxLength);
        }

        private static bool BeOfSameType(OperationCode[] operationCodes)
        {
            var types = operationCodes.Select(x => x).ToList();

            return types.Skip(1).All(p => OperationCodeMetadata.GetCodeType(p) == OperationCodeMetadata.GetCodeType(types.First()));
        }
    }
}