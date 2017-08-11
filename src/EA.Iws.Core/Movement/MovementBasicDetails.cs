namespace EA.Iws.Core.Movement
{
    using System;
    public class MovementBasicDetails
    {
        public Guid Id { get; set; }

        public int Number { get; set; }

        public DateTime ActualDate { get; set; }

        public DateTime? ReceiptDate { get; set; }
    }
}
