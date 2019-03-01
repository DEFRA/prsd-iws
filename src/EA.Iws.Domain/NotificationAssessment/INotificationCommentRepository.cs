namespace EA.Iws.Domain.NotificationAssessment
{
    using System.Threading.Tasks;

    public interface INotificationCommentRepository
    {
        Task<bool> Add(NotificationComment comment);
    }
}
