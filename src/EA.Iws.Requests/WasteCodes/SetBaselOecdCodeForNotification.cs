namespace EA.Iws.Requests.WasteCodes
{
    using System;
    using Core.WasteCodes;
    using Prsd.Core.Mediator;

    public class SetBaselOecdCodeForNotification : IRequest<bool>
    {
        public Guid Id { get; private set; }

        public CodeType CodeType { get; private set; }

        public Guid Code { get; private set; }

        public bool NotApplicable { get; private set; }

        public SetBaselOecdCodeForNotification(Guid id, CodeType codeType, bool notApplicable, Guid code)
        {
            Id = id;
            CodeType = codeType;
            Code = code;
            NotApplicable = notApplicable;
        }
    }
}
