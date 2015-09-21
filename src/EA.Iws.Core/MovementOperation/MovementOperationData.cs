namespace EA.Iws.Core.MovementOperation
{
    using System.Collections.Generic;
    using Movement;
    using Shared;

    public class MovementOperationData
    {
        public IList<MovementData> MovementDatas { get; set; }

        public NotificationType NotificationType { get; set; }
    }
}
