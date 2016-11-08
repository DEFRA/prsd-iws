namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface ICapturedMovementFactory
    {
        Task<Movement> Create(Guid notificationId, 
            int number, 
            DateTime? prenotificationDate, 
            DateTime actualShipmentDate, 
            bool hasNoPrenotification);
    }
}
