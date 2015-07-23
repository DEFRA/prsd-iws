namespace EA.Iws.RequestHandlers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Security;
    using System.Threading.Tasks;
    using Core.Admin;
    using DataAccess;
    using Domain;
    using Prsd.Core.Domain;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Admin.UserAdministration;

    internal class SetDatesHandler : IRequestHandler<SetDates, Guid>
    {
        private readonly IwsContext context;

        public SetDatesHandler(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetDates message)
        {
            return new Guid();
        }
    }
}
