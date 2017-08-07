namespace EA.Iws.Requests.Movement
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Movement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideShipmentData)]
    public class SetMovementReceiptAndRecoveryData : IRequest<Unit>
    {
        public SetMovementReceiptAndRecoveryData(MovementReceiptAndRecoveryData data)
        {
            Data = data;
        }

        public MovementReceiptAndRecoveryData Data { get; private set; }
    }
}
