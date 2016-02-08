namespace EA.Iws.Web.Tests.Unit.Controllers.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.NotificationApplication.Controllers;
    using Areas.NotificationApplication.ViewModels.Facility;
    using Core.Facilities;
    using Core.Notification;
    using Core.Shared;
    using FakeItEasy;
    using Mappings;
    using Prsd.Core.Mediator;
    using Requests.Facilities;
    using Requests.Notification;
    using Requests.Shared;
    using Web.ViewModels.Shared;
    using Xunit;

    public class FacilityControllerTests
    {
        private readonly IMediator mediator;
        private readonly Guid notificationId = new Guid("4AB23CDF-9B24-4598-A302-A69EBB5F2152");
        private readonly Guid facilityId = new Guid("2196585B-F0F0-4A01-BC2F-EB8191B30FC6");
        private readonly FacilityController facilityController;
        private readonly Guid facilityId2 = new Guid("D8991991-64A7-4101-A3A2-2F6B538A0A7A");

        public FacilityControllerTests()
        {
            mediator = A.Fake<IMediator>();

            A.CallTo(() => mediator.SendAsync(A<GetCountries>._)).Returns(new List<CountryData>
            {
                new CountryData
                {
                    Id = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    Name = "United Kingdom"
                },
                new CountryData
                {
                    Id = new Guid("29B0D09E-BA77-49FB-AF95-4171408C07C9"),
                    Name = "Germany"
                }
            });

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetNotificationBasicInfo>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new NotificationBasicInfo
                {
                    CompetentAuthority = UKCompetentAuthority.England,
                    NotificationId = notificationId,
                    NotificationNumber = "GB 0001 002000",
                    NotificationType = NotificationType.Recovery
                });

            facilityController = new FacilityController(mediator, new AddAddressBookEntryMap());
        }

        private AddFacilityViewModel CreateValidAddFacility()
        {
            return new AddFacilityViewModel
            {
                Address = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    CountryName = "United Kingdom",
                    PostalCode = "postcode",
                    Region = "region",
                    StreetOrSuburb = "street",
                    TownOrCity = "town"
                },
                Business = new BusinessTypeViewModel
                {
                    RegistrationNumber = "12345",
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FirstName = "first",
                    LastName = "last",
                    Telephone = "123"
                },
                NotificationId = notificationId
            };
        }

        private EditFacilityViewModel CreateValidEditFacility()
        {
            return new EditFacilityViewModel
            {
                Address = new AddressData
                {
                    Address2 = "address2",
                    CountryId = new Guid("4345FB05-F7DF-4E16-939C-C09FCA5C7D7B"),
                    CountryName = "United Kingdom",
                    PostalCode = "postcode",
                    Region = "region",
                    StreetOrSuburb = "street",
                    TownOrCity = "town"
                },
                Business = new BusinessTypeViewModel
                {
                    RegistrationNumber = "12345",
                    BusinessType = BusinessType.SoleTrader,
                    Name = "business name"
                },
                Contact = new ContactData
                {
                    Email = "email@address.com",
                    FirstName = "first",
                    LastName = "last",
                    Telephone = "123"
                },
                NotificationId = notificationId,
                FacilityId = facilityId
            };
        }

        private FacilityData CreateFacility(Guid facilityId)
        {
            return new FacilityData
            {
                Address = new AddressData(),
                Business = new BusinessInfoData(),
                Contact = new ContactData(),
                Id = facilityId,
                IsActualSiteOfTreatment = facilityId == this.facilityId,
                NotificationId = notificationId
            };
        }

        [Fact]
        public async Task Add_ReturnsView()
        {
            var result = await facilityController.Add(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Add_GetsNotificationBasicInfo()
        {
            await facilityController.Add(notificationId, null);

            A.CallTo(() => mediator.SendAsync(A<GetNotificationBasicInfo>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Add_InvalidModel_ReturnsView()
        {
            var model = new AddFacilityViewModel();

            facilityController.ModelState.AddModelError("Test", "Error");

            var result = await facilityController.Add(model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Add_ValidModel_AddsFacilityToNotification()
        {
            var model = CreateValidAddFacility();

            await facilityController.Add(model);

            A.CallTo(() => mediator.SendAsync(A<AddFacilityToNotification>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Add_ValidModel_RedirectsToList()
        {
            var model = CreateValidAddFacility();

            var result = await facilityController.Add(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = CreateValidAddFacility();

            var result = await facilityController.Add(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = CreateValidAddFacility();

            var result = await facilityController.Add(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Add_ValidModel_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = CreateValidAddFacility();

            var result = await facilityController.Add(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ReturnsView()
        {
            var result = await facilityController.Edit(notificationId, facilityId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_GetsNotificationBasicInfo()
        {
            await facilityController.Edit(notificationId, facilityId);

            A.CallTo(() => mediator.SendAsync(A<GetNotificationBasicInfo>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Edit_InvalidModel_ReturnsView()
        {
            var model = new EditFacilityViewModel();

            facilityController.ModelState.AddModelError("Test", "Error");

            var result = await facilityController.Edit(model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task Edit_ValidModel_EditsFacilityForNotification()
        {
            var model = CreateValidEditFacility();

            await facilityController.Edit(model);

            A.CallTo(() => mediator.SendAsync(A<UpdateFacilityForNotification>._)).MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Edit_ValidModel_RedirectsToList()
        {
            var model = CreateValidEditFacility();

            var result = await facilityController.Edit(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = CreateValidEditFacility();

            var result = await facilityController.Edit(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = CreateValidEditFacility();

            var result = await facilityController.Edit(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Edit_ValidModel_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = CreateValidEditFacility();

            var result = await facilityController.Edit(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task List_ReturnsView()
        {
            var result = await facilityController.List(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task List_GetsFacilitiesByNotificationId()
        {
            await facilityController.List(notificationId);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetFacilitiesByNotificationId>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task List_GetsNotificationBasicInfo()
        {
            await facilityController.List(notificationId);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetNotificationBasicInfo>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfTreatment_ReturnsView()
        {
            var result = await facilityController.SiteOfTreatment(notificationId, null) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task SiteOfTreatment_GetsFacilitiesByNotificationId()
        {
            await facilityController.SiteOfTreatment(notificationId, null);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetFacilitiesByNotificationId>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfTreatment_GetsNotificationBasicInfo()
        {
            await facilityController.SiteOfTreatment(notificationId, null);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<GetNotificationBasicInfo>.That.Matches(p => p.NotificationId == notificationId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfTreatment_Post_SetsSiteOfTreatment()
        {
            var model = new SiteOfTreatmentViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfTreatment = facilityId
            };

            await facilityController.SiteOfTreatment(model, null);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<SetActualSiteOfTreatment>.That.Matches(p => p.NotificationId == notificationId && p.FacilityId == facilityId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task SiteOfTreatment_BackToListTrue_ReturnsList()
        {
            var model = new SiteOfTreatmentViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfTreatment = facilityId
            };

            var result = await facilityController.SiteOfTreatment(model, true) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task SiteOfTreatment_BackToListFalse_RecoveryReturnsRecoveryPreconsent()
        {
            var model = new SiteOfTreatmentViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfTreatment = facilityId,
                NotificationType = NotificationType.Recovery
            };

            var result = await facilityController.SiteOfTreatment(model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "RecoveryPreconsent", "Facility");
        }

        [Fact]
        public async Task SiteOfTreatment_BackToListFalse_DisposalReturnsOperationCodes()
        {
            var model = new SiteOfTreatmentViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfTreatment = facilityId,
                NotificationType = NotificationType.Disposal
            };

            var result = await facilityController.SiteOfTreatment(model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "OperationCodes", "WasteOperations");
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        [InlineData(null)]
        public async Task SiteOfTreatment_BackToListTrue_IgnoresBackToOverview(bool? backToOverview)
        {
            var model = new SiteOfTreatmentViewModel
            {
                NotificationId = notificationId,
                SelectedSiteOfTreatment = facilityId,
                NotificationType = NotificationType.Recovery
            };

            var result = await facilityController.SiteOfTreatment(model, true, backToOverview) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task RecoveryPreconsent_DisposalOperation_RedirectsToOperationCodes()
        {
            A.CallTo(() => mediator.SendAsync(A<GetIsPreconsentedRecoveryFacility>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new PreconsentedFacilityData
                {
                    NotificationId = notificationId,
                    IsPreconsentedRecoveryFacility = false,
                    NotificationType = NotificationType.Disposal
                });

            var result = await facilityController.RecoveryPreconsent(notificationId) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "OperationCodes", "WasteOperations");
        }

        [Fact]
        public async Task RecoveryPreconsent_RecoveryOperation_ReturnsView()
        {
            A.CallTo(() => mediator.SendAsync(A<GetIsPreconsentedRecoveryFacility>.That.Matches(p => p.NotificationId == notificationId)))
                .Returns(new PreconsentedFacilityData
                {
                    NotificationId = notificationId,
                    IsPreconsentedRecoveryFacility = false,
                    NotificationType = NotificationType.Recovery
                });

            var result = await facilityController.RecoveryPreconsent(notificationId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task RecoveryPreconsent_PostInvalidModel_ReturnsView()
        {
            var model = new TrueFalseViewModel();
            facilityController.ModelState.AddModelError("Test", "Error");

            var result = await facilityController.RecoveryPreconsent(notificationId, model) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
        }

        [Fact]
        public async Task RecoveryPreconsent_PostValidModel_SetsPreconsentedFacility()
        {
            var model = new TrueFalseViewModel { Value = true };

            await facilityController.RecoveryPreconsent(notificationId, model);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<SetPreconsentedRecoveryFacility>.That.Matches(
                            p => p.NotificationId == notificationId && p.IsPreconsentedRecoveryFacility)))
                            .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task RecoveryPreconsent_PostValidModel_RedirectsToOperationCodes()
        {
            var model = new TrueFalseViewModel { Value = true };

            var result = await facilityController.RecoveryPreconsent(notificationId, model) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "OperationCodes", "WasteOperations");
        }

        [Fact]
        public async Task RecoveryPreconsent_Post_BackToOverviewTrue_RedirectsToOverview()
        {
            var model = new TrueFalseViewModel { Value = true };

            var result = await facilityController.RecoveryPreconsent(notificationId, model, true) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "Index", "Home");
        }

        [Fact]
        public async Task RecoveryPreconsent_Post_BackToOverviewFalse_RedirectToOperationCodes()
        {
            var model = new TrueFalseViewModel { Value = true };

            var result = await facilityController.RecoveryPreconsent(notificationId, model, false) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "OperationCodes", "WasteOperations");
        }
        
        [Fact]
        public async Task RecoveryPreconsent_Post_BackToOverviewNull_RedirectToOperationCodes()
        {
            var model = new TrueFalseViewModel { Value = true };

            var result = await facilityController.RecoveryPreconsent(notificationId, model, null) as RedirectToRouteResult;

            RouteAssert.RoutesTo(result.RouteValues, "OperationCodes", "WasteOperations");
        }
        
        [Fact]
        public async Task Remove_MultipleFacilitiesRemoveSiteOfTreatment_ReturnsViewWithError()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilitiesByNotificationId>._)).Returns(
                new List<FacilityData>
                {
                    CreateFacility(facilityId),
                    CreateFacility(facilityId2)
                });

            var result = await facilityController.Remove(notificationId, facilityId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.NotNull(result.ViewBag.Error);
        }

        [Fact]
        public async Task Remove_MultipleFacilitiesRemoveNotSiteOfTreatment_ReturnsViewWithNoError()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilitiesByNotificationId>._)).Returns(
                new List<FacilityData>
                {
                    CreateFacility(facilityId),
                    CreateFacility(facilityId2)
                });

            var result = await facilityController.Remove(notificationId, facilityId2) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Null(result.ViewBag.Error);
        }

        [Fact]
        public async Task Remove_SingleFacility_ReturnsViewWithNoError()
        {
            A.CallTo(() => mediator.SendAsync(A<GetFacilitiesByNotificationId>._)).Returns(
                new List<FacilityData>
                {
                    CreateFacility(facilityId)
                });

            var result = await facilityController.Remove(notificationId, facilityId) as ViewResult;

            Assert.Equal(string.Empty, result.ViewName);
            Assert.Null(result.ViewBag.Error);
        }

        [Fact]
        public async Task Remove_CallsClient()
        {
            var model = new RemoveFacilityViewModel
            {
                NotificationId = notificationId,
                FacilityId = facilityId
            };

            await facilityController.Remove(model);

            A.CallTo(
                () =>
                    mediator.SendAsync(A<DeleteFacilityForNotification>.That.Matches(
                            p => p.NotificationId == notificationId && p.FacilityId == facilityId)))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task Remove_ReturnsList()
        {
            var model = new RemoveFacilityViewModel
            {
                NotificationId = notificationId,
                FacilityId = facilityId
            };

            var result = await facilityController.Remove(model) as RedirectToRouteResult;

            Assert.Equal("List", result.RouteValues["action"]);
        }

        [Fact]
        public async Task Remove_WithBackToOverviewTrue_MaintainsRouteValue()
        {
            var model = new RemoveFacilityViewModel
            {
                NotificationId = notificationId,
                FacilityId = facilityId
            };

            var result = await facilityController.Remove(model, true) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.True(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_WithBackToOverviewFalse_MaintainsRouteValue()
        {
            var model = new RemoveFacilityViewModel
            {
                NotificationId = notificationId,
                FacilityId = facilityId
            };

            var result = await facilityController.Remove(model, false) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }

        [Fact]
        public async Task Remove_WithBackToOverviewNull_DefaultsRouteValueToFalse()
        {
            var model = new RemoveFacilityViewModel
            {
                NotificationId = notificationId,
                FacilityId = facilityId
            };

            var result = await facilityController.Remove(model, null) as RedirectToRouteResult;

            var backToOverviewKey = "backToOverview";
            Assert.True(result.RouteValues.ContainsKey(backToOverviewKey));
            Assert.False(Convert.ToBoolean(result.RouteValues[backToOverviewKey]));
        }
    }
}