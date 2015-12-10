namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Domain;
    using Domain.NotificationApplication;
    using FluentValidation;
    using Prsd.Core.Domain;

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
        }

        private static bool BeOfSameType(int[] operationCodes)
        {
            var types = operationCodes.Select(x => Enumeration.FromValue<OperationCode>(x).NotificationType).ToArray();

            return types.All(p => p == types.First());
        }
    }
}