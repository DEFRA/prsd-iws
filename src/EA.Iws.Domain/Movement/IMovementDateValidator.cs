namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IMovementDateValidator
    {
        Task EnsureDateValid(Movement movement, DateTime newDate);
    }
}