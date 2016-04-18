namespace EA.Iws.Domain.Movement
{
    using System;
    using Core.Movement;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MovementStatusChange : Entity
    {
        protected MovementStatusChange()
        {
        }

        public MovementStatusChange(MovementStatus status, User user)
        {
            Guard.ArgumentNotNull(() => user, user);

            User = user;
            Status = status;
            ChangeDate = new DateTimeOffset(SystemTime.UtcNow, TimeSpan.Zero);
        }

        public MovementStatus Status { get; private set; }
        public User User { get; private set; }
        public DateTimeOffset ChangeDate { get; private set; }
    }
}
