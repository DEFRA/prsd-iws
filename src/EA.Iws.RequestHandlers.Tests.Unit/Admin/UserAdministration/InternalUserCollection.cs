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
        public static readonly Guid AdminPendingUserId = new Guid("89EBDF61-0B9B-459A-AE14-368A5B9928EB");
        public static readonly Guid AdminApprovedUserId = new Guid("AFBB7102-DD59-4BDA-9C59-93AFA768AE50");
        public static readonly Guid AdminRejectedUserId = new Guid("25E2A9F6-14C0-462E-A0CC-E58665819B50");
        public static readonly Guid ThisUserAdminPendingUserId = new Guid("29BA2B1E-65CF-4AC6-968C-B2E72385B8E5");
        public static readonly Guid ThisUserAdminApprovedUserId = new Guid("E8DA4222-A18B-4E73-A750-BAEC509174AC");
        public static readonly Guid NonAdminUserId = new Guid("4573456B-605B-4629-9FA5-3CD4AA95C46B");

        public static readonly Guid AdminPendingId = new Guid("2842B4DF-4751-4364-888F-7B40E8869D75");
        public static readonly Guid AdminApprovedId = new Guid("462662F0-9EDA-41FC-A0D4-AD986CBF7259");
        public static readonly Guid AdminRejectedId = new Guid("DB78653C-DC68-4A21-97BB-C8A1D1634FC5");
        public static readonly Guid ThisUserAdminPendingId = new Guid("D7A53F47-F4A6-445B-A6C2-F27DD721261D");
        public static readonly Guid ThisUserAdminApprovedId = new Guid("74F4850C-911F-41DF-9517-DDD3E0E4AB75");

        private static readonly Action<InternalUser, InternalUserStatus> SetInternalUserStatus =
            (user, status) => ObjectInstantiator<InternalUser>.SetProperty(u => u.Status, status, user);

        private static readonly Action<User, bool> SetEmailConfirmed =
            (user, b) => ObjectInstantiator<User>.SetProperty(u => u.EmailConfirmed, b, user);

        private readonly Func<Guid, IList<InternalUser>, int> getUserIndexById =
            (guid, users) => { return users.IndexOf(users.Single(u => u.UserId == guid.ToString())); };

        public IList<InternalUser> Users;

        public int AdminPendingIndex
        {
            get { return getUserIndexById(AdminPendingUserId, Users); }
        }

        public int AdminApprovedIndex
        {
            get { return getUserIndexById(AdminApprovedUserId, Users); }
        }

        public int AdminRejectedIndex
        {
            get { return getUserIndexById(AdminRejectedUserId, Users); }
        }

        public int ThisUserAdminPendingIndex
        {
            get { return getUserIndexById(ThisUserAdminPendingUserId, Users); }
        }

        public int ThisUserAdminApprovedIndex
        {
            get { return getUserIndexById(ThisUserAdminApprovedUserId, Users); }
        }

        public InternalUserCollection()
        {
            Users = new[]
            {
                InternalUserFactory.Create(AdminPendingId,
                    UserFactory.Create(AdminPendingUserId, AnyString, AnyString, AnyString,
                        AnyString)),
                InternalUserFactory.Create(AdminApprovedId, UserFactory.Create(AdminApprovedUserId, AnyString, AnyString, AnyString,
                    AnyString)),
                InternalUserFactory.Create(AdminRejectedId, UserFactory.Create(AdminRejectedUserId, AnyString, AnyString, AnyString,
                    AnyString)),
                InternalUserFactory.Create(ThisUserAdminPendingId, UserFactory.Create(ThisUserAdminPendingUserId, AnyString, AnyString, AnyString,
                    AnyString)),
                InternalUserFactory.Create(ThisUserAdminApprovedId, UserFactory.Create(ThisUserAdminApprovedUserId, AnyString, AnyString, AnyString,
                    AnyString))
            };

            foreach (var user in Users)
            {
                SetEmailConfirmed(user.User, true);
            }

            SetInternalUserStatus(Users[AdminPendingIndex], InternalUserStatus.Pending);
            SetInternalUserStatus(Users[AdminApprovedIndex], InternalUserStatus.Approved);
            SetInternalUserStatus(Users[AdminRejectedIndex], InternalUserStatus.Rejected);
            SetInternalUserStatus(Users[ThisUserAdminPendingIndex], InternalUserStatus.Pending);
            SetInternalUserStatus(Users[ThisUserAdminApprovedIndex], InternalUserStatus.Approved);
        }
    }
}