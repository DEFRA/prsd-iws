namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    public class SetBaselOecdCodeForNotification : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public CodeType CodeType { get; private set; }

        public Guid? Code { get; private set; }

        public bool NotApplicable { get; private set; }

        public SetBaselOecdCodeForNotification(Guid id, CodeType codeType, bool notApplicable, Guid? code)
        {
            if (!notApplicable && !code.HasValue)
            {
                throw new InvalidOperationException("You must provide a code to set where you have specified that the code should be applicable.");
            }

            if (notApplicable && code.HasValue)
            {
                throw new InvalidOperationException("Cannot request a code to set where you have also specified that the code should be not applicable.");
            }

            Id = id;
            CodeType = codeType;
            Code = code;
            NotApplicable = notApplicable;
        }
    }
}
