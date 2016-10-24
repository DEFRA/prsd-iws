namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface ITechnologyEmployedRepository
    {
        Task<TechnologyEmployed> GetByNotificaitonId(Guid notificationId);
    }
}