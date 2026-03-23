namespace EA.Iws.Domain.OperationCodes
{
    using EA.Iws.Core.Shared;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IOperationCodeRepository
    {
        Task<IEnumerable<OperationCode>> GetOperationCodes(NotificationType notificationType, bool isInterim);
    }
}
