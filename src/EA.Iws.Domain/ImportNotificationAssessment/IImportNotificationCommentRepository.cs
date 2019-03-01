namespace EA.Iws.Domain.ImportNotificationAssessment
{
    using System.Threading.Tasks;
    public interface IImportNotificationCommentRepository
    {
        Task<bool> Add(ImportNotificationComment comment);
    }
}
