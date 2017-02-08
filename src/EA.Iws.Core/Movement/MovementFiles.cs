namespace EA.Iws.Core.Movement
{
    public class MovementFiles
    {
        public int NumberOfShipments { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public MovementFileData[] FileData { get; set; }
    }
}