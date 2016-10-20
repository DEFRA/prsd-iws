namespace EA.Iws.Core.ImportMovement
{
    using System;

    [Serializable]
    public class ImportCancelMovementData
    {
        public Guid Id { get; set; }

        public int Number { get; set; }
    }
}