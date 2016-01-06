namespace EA.Iws.Domain.Movement
{
    using System;
    using System.Threading.Tasks;

    public interface IUpdatedMovementDateValidator
    {
        Task EnsureDateValid(Movement movement, DateTime newDate);
    }
}