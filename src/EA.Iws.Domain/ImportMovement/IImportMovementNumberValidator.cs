namespace EA.Iws.Domain.ImportMovement
{
    using System;
    using System.Threading.Tasks;

    public interface IImportMovementNumberValidator
    {
        Task<bool> Validate(Guid notificationId, int number);
    }
}
