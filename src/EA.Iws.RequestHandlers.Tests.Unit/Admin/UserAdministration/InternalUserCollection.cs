namespace EA.Iws.RequestHandlers.Tests.Unit.Admin.UserAdministration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Admin;
    using Domain;
    using TestHelpers.Helpers;

    internal class InternalUserCollection
    {
        private static readonly string AnyString = "test";
        public static readonly Guid AdminPendingId = new Guid("89EBDF61-0B9B-459A-AE14-368A5B9928EB");
        public static readonly Guid AdminApprovedId = new Guid("AFBB7102-DD59-4BDA-9C59-93AFA768AE50");
        public static readonly Guid AdminRejectedId = new Guid("25E2A9F6-14C0-462E-A0CC-E58665819B50");
        public static readonly Guid ThisUserAdminPendingId = new Guid("29BA2B1E-65CF-4AC6-968C-B2E72385B8E5");
        public static readonly Guid ThisUserAdminApprovedId = new Guid("E8DA4222-A18B-4E73-A750-BAEC509174AC");
        public static readonly Guid NonAdminUserId = new Guid("4573456B-605B-4629-9FA5-3CD4AA95C46B");

        private static readonly Action<User, bool> SetIsAdminForUser =
            (user, isAdmin) => ObjectInstantiator<User>.SetProperty(u => u.IsInternal, isAdmin, user);
        private static readonly Action<User, InternalUserStatus> SetInternalUserStatus =
            (user, status) => ObjectInstantiator<User>.SetProperty(u => u.InternalUserStatus, status, user);
        private static readonly Action<User, bool> SetEmailConfirmed =
            (user, b) => ObjectInstantiator<User>.SetProperty(u => u.EmailConfirmed, b, user);

        private readonly Func<Guid, IList<User>, int> getUserIndexById =
            (guid, users) => { return users.IndexOf(users.Single(u => u.Id == guid.ToString())); };

        public IList<User> Users;

        public int AdminPendingIndex
        {
            get { return getUserIndexById(AdminPendingId, Users); }
        }

        public int AdminApprovedIndex
        {
            get { return getUserIndexById(AdminApprovedId, Users); }
        }

        public int AdminRejectedIndex
        {
            get { return getUserIndexById(AdminRejectedId, Users); }
        }

        public int ThisUserAdminPendingIndex
        {
            get { return getUserIndexById(ThisUserAdminPendingId, Users); }
        }

        public int ThisUserAdminApprovedIndex
        {
            get { return getUserIndexById(ThisUserAdminApprovedId, Users); }
        }

        public int NonAdminUserIndex
        {
            get { return getUserIndexById(NonAdminUserId, Users); }
        }

        public InternalUserCollection()
        {
            Users = new[]
            {
                UserFactory.Create(AdminPendingId, AnyString, AnyString, AnyString,
                    AnyString),
                UserFactory.Create(AdminApprovedId, AnyString, AnyString, AnyString,
                    AnyString),
                UserFactory.Create(AdminRejectedId, AnyString, AnyString, AnyString,
                    AnyString),  
                UserFactory.Create(ThisUserAdminPendingId, AnyString, AnyString, AnyString,
                    AnyString),
                UserFactory.Create(ThisUserAdminApprovedId, AnyString, AnyString, AnyString,
                    AnyString),
                UserFactory.Create(NonAdminUserId, AnyString, AnyString, AnyString,
                    AnyString)
            };

            foreach (var user in Users)
            {
                SetEmailConfirmed(user, true);
            }

            SetIsAdminForUser(Users[AdminPendingIndex], true);
            SetInternalUserStatus(Users[AdminPendingIndex], InternalUserStatus.Pending);

            SetIsAdminForUser(Users[AdminApprovedIndex], true);
            SetInternalUserStatus(Users[AdminApprovedIndex], InternalUserStatus.Approved);

            SetIsAdminForUser(Users[AdminRejectedIndex], true);
            SetInternalUserStatus(Users[AdminRejectedIndex], InternalUserStatus.Rejected);

            SetIsAdminForUser(Users[ThisUserAdminPendingIndex], true);
            SetInternalUserStatus(Users[ThisUserAdminPendingIndex], InternalUserStatus.Pending);

            SetIsAdminForUser(Users[ThisUserAdminApprovedIndex], true);
            SetInternalUserStatus(Users[ThisUserAdminApprovedIndex], InternalUserStatus.Approved);

            SetIsAdminForUser(Users[NonAdminUserIndex], false);
        }
    }
}