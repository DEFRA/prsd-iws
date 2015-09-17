namespace EA.Iws.Domain.Movement
{
    public interface IActiveMovement
    {
        bool IsActive(Movement movement);
    }
}
