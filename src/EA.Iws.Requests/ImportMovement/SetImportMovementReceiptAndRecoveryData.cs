namespace EA.Iws.Requests.ImportMovement
{
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using EA.Iws.Core.ImportMovement;
    using Prsd.Core.Mediator;

    [RequestAuthorization(UserAdministrationPermissions.CanOverrideShipmentData)]
    public class SetImportMovementReceiptAndRecoveryData : IRequest<Unit>
    {
        public SetImportMovementReceiptAndRecoveryData(ImportMovementSummaryData data)
        {
            Data = data;
        }

        public ImportMovementSummaryData Data { get; private set; }
    }
}
