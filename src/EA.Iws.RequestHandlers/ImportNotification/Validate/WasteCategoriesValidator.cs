namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using EA.Iws.Core.ImportNotification.Draft;
    using FluentValidation;

    internal class WasteCategoriesValidator : AbstractValidator<WasteCategories>
    {
        public WasteCategoriesValidator()
        {
            RuleFor(x => x.WasteCategoryType)
                .Must(BeEntered)
                .WithLocalizedMessage(() => WasteCategoriesValidatorResources.WasteCategoryTypeRequred);
        }

        private static bool BeEntered(Core.WasteType.WasteCategoryType? arg)
        {
            return arg.HasValue && arg.Value != default(Core.WasteType.WasteCategoryType);
        }
    }
}
