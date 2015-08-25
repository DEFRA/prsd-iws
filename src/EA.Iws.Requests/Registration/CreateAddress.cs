namespace EA.Iws.Requests.Registration
{
    using System;
    using Core.Shared;
    using Prsd.Core.Mediator;

    public class CreateAddress : IRequest<Guid>
    {
        public AddressData Address { get; set; }

        public string UserId { get; set; }
    }
}
