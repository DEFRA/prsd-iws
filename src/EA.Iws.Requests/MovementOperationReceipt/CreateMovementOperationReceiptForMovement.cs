namespace EA.Iws.Requests.MovementOperationReceipt
{
    using EA.Prsd.Core.Mediator;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CreateMovementOperationReceiptForMovement : IRequest<bool>
    {
        public Guid MovementId { get; private set; }
        public DateTime DateComplete { get; private set; }

        public CreateMovementOperationReceiptForMovement(Guid movementId, DateTime dateComplete)
        {
            MovementId = movementId;
            DateComplete = dateComplete;
        }
    }
}
