namespace EA.Iws.DataAccess.Tests.Integration
{
    using System;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core.Domain;
    using TestHelpers.Helpers;
    using Xunit;

    public class RelationshipDeletionIntegration
    {
        private readonly IUserContext userContext;
        private IwsContext context;
        private Guid userId;
        private NotificationApplication notification;
        private Guid notificationId;

        public RelationshipDeletionIntegration()
        {
            userId = new Guid("5BA5B2CE-A29C-4B94-A528-567F636CA456");
            userContext = A.Fake<IUserContext>();
            A.CallTo(() => userContext.UserId).Returns(userId);
            context = new IwsContext(userContext);
            notification = new NotificationApplication(userId, NotificationType.Recovery,
                UKCompetentAuthority.England, 0);
        }

        [Fact]
        public async Task UpdateOrganisation_BusinessType_Should_Not_Remove_Organisation()
        {
            //Create new user
            var newUser = new User(userId.ToString(), "testFirst", "testLast", "9999", "testfirst@testlast.com");

            //Create new org
            var country = context.Countries.Single(c => c.IsoAlpha2Code.Equals("gb"));
            var address = TestAddress(country);
            var org = new Organisation("SFW Ltd", address, BusinessType.LimitedCompany);

            try
            {
                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                context.Organisations.Add(org);
                await context.SaveChangesAsync();

                //Assign org to user
                newUser.LinkToOrganisation(org);
                await context.SaveChangesAsync();

                //Hold OrgID of newly created entity
                var oldOrgId = org.Id;

                //Update org with change in Business Type
                org = new Organisation("Name Changed", address, BusinessType.SoleTrader);
                context.Organisations.Add(org);
                await context.SaveChangesAsync();

                //Update user with newly created org2
                var user = await context.Users.SingleAsync(u => u.Id == newUser.Id.ToString());
                user.UpdateOrganisationOfUser(org);
                await context.SaveChangesAsync();
                Assert.True(user.Organisation.Id == org.Id);

                //Both Orgs should have different OrgIds
                Assert.False(oldOrgId == org.Id);

                //Check if old org exists
                var oldExists = context.Organisations.Any(x => x.Id == oldOrgId);
                Assert.True(oldExists);

                //Check if new org exists
                var newExists = context.Organisations.Any(x => x.Id == org.Id);
                Assert.True(newExists);
            }
            finally
            {
                CleanUp(org);

                context.Entry(newUser).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        [Fact]
        public async Task UpdatePackagingInfoReplacesItems()
        {
            var packagingInfo1 = PackagingInfo.CreatePackagingInfo(PackagingType.Bag);
            var packagingInfo2 = PackagingInfo.CreatePackagingInfo(PackagingType.Box);
            var packagingInfo3 = PackagingInfo.CreatePackagingInfo(PackagingType.Bulk);

            try
            {
                context.NotificationApplications.Add(notification);

                await context.SaveChangesAsync();

                notificationId = notification.Id;

                notification.SetPackagingInfo(new[]
                {
                    packagingInfo1
                });

                await context.SaveChangesAsync();

                var count =
                    await context.Database.SqlQuery<int>(
                        "select count(id) from business.PackagingInfo where NotificationId = @id",
                        new SqlParameter("id", notificationId)).SingleAsync();

                Assert.Equal(1, count);

                notification.SetPackagingInfo(new[]
                {
                    packagingInfo2,
                    packagingInfo3
                });

                await context.SaveChangesAsync();

                count =
                    await context.Database.SqlQuery<int>(
                        "select count(id) from business.PackagingInfo where NotificationId = @id",
                        new SqlParameter("id", notificationId)).SingleAsync();

                Assert.Equal(2, count);
            }
            finally
            {
                if (context.Entry(packagingInfo1).State != EntityState.Detached)
                {
                    context.Entry(packagingInfo1).State = EntityState.Deleted;
                }

                if (context.Entry(packagingInfo2).State != EntityState.Detached)
                {
                    context.Entry(packagingInfo2).State = EntityState.Deleted;
                }

                if (context.Entry(packagingInfo3).State != EntityState.Detached)
                {
                    context.Entry(packagingInfo3).State = EntityState.Deleted;
                }

                context.DeleteOnCommit(notification);

                context.SaveChanges();
            }
        }

        [Fact]
        public async Task UpdateExporterDeletesOldExporter()
        {
            try
            {
                context.NotificationApplications.Add(notification);

                await context.SaveChangesAsync();

                notificationId = notification.Id;

                notification.SetExporter(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

                await context.SaveChangesAsync();

                var count =
                    await context.Database.SqlQuery<int>(
                        "select count(id) from business.Exporter where NotificationId = @id",
                        new SqlParameter("id", notificationId)).SingleAsync();

                Assert.Equal(1, count);

                notification.SetExporter(ObjectFactory.CreateEmptyBusiness(), ObjectFactory.CreateDefaultAddress(), ObjectFactory.CreateEmptyContact());

                await context.SaveChangesAsync();

                count =
                    await context.Database.SqlQuery<int>(
                        "select count(id) from business.Exporter where NotificationId = @id",
                        new SqlParameter("id", notificationId)).SingleAsync();

                Assert.Equal(1, count);
            }
            finally
            {
                context.DeleteOnCommit(notification);

                context.SaveChanges();
            }
        }

        private static Address TestAddress(Country country)
        {
            return new Address("1", "test street", null, "Woking", null, "GU22 7UM", country.Name);
        }

        private void CleanUp(Organisation organisation)
        {
            context.Organisations.Remove(organisation);

            context.SaveChanges();
        }
    }
}