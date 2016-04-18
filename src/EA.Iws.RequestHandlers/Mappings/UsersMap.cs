namespace EA.Iws.RequestHandlers.Mappings
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Admin;
    using Domain;
    using Prsd.Core.Mapper;

    internal class UsersMap : IMap<IEnumerable<User>, IEnumerable<ChangeUserData>>
    {
        private readonly IMap<User, ChangeUserData> mapper;

        public UsersMap(IMap<User, ChangeUserData> mapper)
        {
            this.mapper = mapper;
        }

        public IEnumerable<ChangeUserData> Map(IEnumerable<User> source)
        {
            return source.Select(u => mapper.Map(u)).ToList();
        }
    }
}
