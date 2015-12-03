namespace EA.Iws.RequestHandlers.ImportNotification.Validate
{
    using System.Linq;
    using Core.ImportNotification.Draft;
    using Domain.NotificationApplication;
    using FluentValidation;
    using Prsd.Core.Domain;

    internal class WasteOperationValidator : AbstractValidator<WasteOperation>
    {
        public WasteOperationValidator()
        {
            RuleFor(x => x.OperationCodes).NotEmpty().Must(BeOfSameType);
        }

        private static bool BeOfSameType(int[] operationCodes)
        {
            var types = operationCodes.Select(x => Enumeration.FromValue<OperationCode>(x).NotificationType).ToArray();

            return types.All(p => p == types.First());
        }
    }
}