namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportNotification.Draft;
    using FluentValidation;

    internal class FacilityCollectionValidator : AbstractValidator<FacilityCollection>
    {
        public FacilityCollectionValidator(IValidator<Facility> facilityValidator)
        {
            RuleFor(x => x.Facilities)
                .SetCollectionValidator(facilityValidator)
                .NotEmpty()
                .Must(HaveSiteOfTreatment);
        }

        private static bool HaveSiteOfTreatment(IEnumerable<Facility> facilities)
        {
            return facilities.Count(p => p.IsActualSite) == 1;
        }
    }
}