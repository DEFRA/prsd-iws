namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using Core.ImportNotification.Draft;
    using FluentValidation;

    internal class ChemicalCompositionValidator : AbstractValidator<ChemicalComposition>
    {
        public ChemicalCompositionValidator()
        {
            RuleFor(x => x.Composition)
                .Must(BeEntered)
                .WithLocalizedMessage(() => ChemicalCompositionValidatorResources.CompositionRequred);
        }

        private static bool BeEntered(Core.WasteType.ChemicalComposition? arg)
        {
            return arg.HasValue && arg.Value != default(Core.WasteType.ChemicalComposition);
        }
    }
}