namespace EA.Iws.Domain.ImportNotification
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IImportNotificationSearchRepository
    {
        Task<IEnumerable<ImportNotificationSearchResult>> SearchBySearchTerm(string searchTerm);
    }
}
