namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportNotification.Draft;
    using FluentValidation;

    internal class WasteTypeValidator : AbstractValidator<WasteType>
    {
        public WasteTypeValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithLocalizedMessage(() => WasteTypeValidatorResources.WasteTypeNameNotEmpty);

            RuleFor(x => x.SelectedBaselCode)
                .Must(HaveBaselCodeOrNotListed)
                .WithLocalizedMessage(() => WasteTypeValidatorResources.BaselCodeSelectedOrNotListed);

            RuleFor(x => x.SelectedEwcCodes)
                .NotEmpty()
                .WithLocalizedMessage(() => WasteTypeValidatorResources.EwcCodeNotEmpty);

            RuleFor(x => x.SelectedHCodes)
                .Must(HaveHCodesOrNotApplicable)
                .WithLocalizedMessage(() => WasteTypeValidatorResources.HCodeSelectedOrNotApplicable);

            RuleFor(x => x.SelectedUnClasses)
                .Must(HaveUnClassesOrNotApplicable)
                .WithLocalizedMessage(() => WasteTypeValidatorResources.UnClassSelectedOrNotApplicable);

            RuleFor(x => x.SelectedYCodes)
                .Must(HaveYCodesOrNotApplicable)
                .WithLocalizedMessage(() => WasteTypeValidatorResources.YCodeSelectedOrNotApplicable);
        }

        private bool HaveYCodesOrNotApplicable(WasteType instance, List<Guid> selectedYCodes)
        {
            return instance.YCodeNotApplicable || (selectedYCodes != null && selectedYCodes.Any());
        }

        private bool HaveUnClassesOrNotApplicable(WasteType instance, List<Guid> selectedUnClasses)
        {
            return instance.UnClassNotApplicable || (selectedUnClasses != null && selectedUnClasses.Any());
        }

        private static bool HaveHCodesOrNotApplicable(WasteType instance, List<Guid> selectedHCodes)
        {
            return instance.HCodeNotApplicable || (selectedHCodes != null && selectedHCodes.Any());
        }

        private static bool HaveBaselCodeOrNotListed(WasteType instance, Guid? selectedBaselCode)
        {
            return instance.BaselCodeNotListed || selectedBaselCode.GetValueOrDefault() != default(Guid);
        }
    }
}