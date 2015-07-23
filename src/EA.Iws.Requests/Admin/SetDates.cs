namespace EA.Iws.Requests.Admin
{
    using System;
    using Prsd.Core.Mediator;

    public class SetDates : IRequest<Guid>
    {
        public DateTime DecisionDate { get; set; }
    }
}
