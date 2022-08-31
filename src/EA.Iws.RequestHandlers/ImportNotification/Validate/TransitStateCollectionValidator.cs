namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.ImportNotification.Draft;
    using FluentValidation;

    internal class TransitStateCollectionValidator : AbstractValidator<TransitStateCollection>
    {
        [System.Obsolete]
        public TransitStateCollectionValidator(IValidator<TransitState> transitStateValidator)
        {
            RuleFor(x => x.HasNoTransitStates).Cascade(CascadeMode.StopOnFirstFailure)
                .Must(BeTrueOrContainTransitStates)
                .WithMessage(x => TransitStateCollectionValidatorResources.TransitStatesEmptyAndNotSelected);

            RuleForEach(x => x.TransitStates)
                //.Must(BeConsecutivelyOrdered)
                .SetValidator(transitStateValidator);
        }

        private bool BeTrueOrContainTransitStates(TransitStateCollection transitStateCollection, bool hasNoTransitStates)
        {
            if (hasNoTransitStates && transitStateCollection.TransitStates.Count == 0)
            {
                return true;
            }

            if (!hasNoTransitStates && transitStateCollection.TransitStates.Count > 0)
            {
                return true;
            }

            return false;
        }

        private bool BeConsecutivelyOrdered(IList<TransitState> transitStates)
        {
            if (transitStates.Count > 0)
            {
                if (transitStates.Count == 1 && transitStates[0].OrdinalPosition == 1)
                {
                    return true;
                }

                return !Enumerable.Range(1, transitStates.Count).Except(transitStates.Select(ts => ts.OrdinalPosition)).Any();
            }

            return true;
        }
    }
}