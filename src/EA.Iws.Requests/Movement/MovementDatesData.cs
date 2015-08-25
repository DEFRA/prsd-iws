namespace EA.Iws.Requests.Movement
{
    using System;

    public class MovementDatesData
    {
        public Guid MovementId { get; set; }
        public DateTime FirstDate { get; set; }
        public DateTime LastDate { get; set; }
        public DateTime ActualDate { get; set; }
    }
}
