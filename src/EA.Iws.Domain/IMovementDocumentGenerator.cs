namespace EA.Iws.Domain
{
    public interface IMovementDocumentGenerator
    {
        byte[] Generate(Movement.Movement movement);
    }
}
