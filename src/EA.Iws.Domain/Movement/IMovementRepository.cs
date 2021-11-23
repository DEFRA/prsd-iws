namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Movement;

    public interface IMovementRepository
    {
        Task<Movement> GetById(Guid movementId);

        Task<IEnumerable<Movement>> GetMovementsByIds(Guid notificationId, IEnumerable<Guid> movementIds);

        Task<IEnumerable<Movement>> GetAllMovements(Guid notificationId);

        Task<IEnumerable<Movement>> GetMovementsByStatus(Guid notificationId, MovementStatus status);

        Task<IEnumerable<Movement>> GetCancellableMovements(Guid notificationId);

        Task<IEnumerable<Movement>> GetPagedMovements(Guid notificationId, int pageNumber, int pageSize);

        Task<IEnumerable<Movement>> GetPagedMovementsByStatus(Guid notificationId, MovementStatus status, int pageNumber, int pageSize);

        Task<int> GetTotalNumberOfMovements(Guid notificationId, MovementStatus? status);

        Task<Movement> GetByNumberOrDefault(int movementNumber, Guid notificationId);

        Task<IEnumerable<Movement>> GetAllActiveMovements(Guid notificationId);

        void Add(Movement movement);

        Task<int> GetLatestMovementNumber(Guid notificationId);

        Task<bool> DeleteById(Guid movementId);

        Task<IEnumerable<Movement>> GetRejectedMovements(Guid notificationId);

        Task SetMovementReceiptAndRecoveryData(MovementReceiptAndRecoveryData data, Guid createdBy);

        Task<IEnumerable<Movement>> GetAllActiveMovementsForReceipt(Guid notificationId);

        Task<IEnumerable<Movement>> GetAllActiveMovementsForRecovery(Guid notificationId);

        Task<IEnumerable<Movement>> GetAllActiveMovementsForReceiptAndRecovery(Guid notificationId);
    }
}