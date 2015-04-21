namespace EA.Iws.Core.Domain
{
    using System;

    public interface IUserContext
    {
        Guid UserId { get; }
    }
}