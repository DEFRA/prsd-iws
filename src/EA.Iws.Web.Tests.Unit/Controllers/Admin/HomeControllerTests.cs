﻿namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Areas.Admin.Controllers;
    using Areas.Admin.ViewModels;
    using Areas.Admin.ViewModels.Home;
    using Core.Admin.Search;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Admin;
    using Requests.Admin.Search;
    using Xunit;

    public class HomeControllerTests
    {
        private readonly HomeController controller;
        private readonly IMediator mediator;
        private readonly BasicSearchViewModel postModel;
        private readonly IList<BasicSearchResult> searchResults;

        public HomeControllerTests()
        {
            mediator = A.Fake<IMediator>();

            controller = new HomeController(mediator);

            postModel = new BasicSearchViewModel
            {
                SearchTerm = "GB001"
            };

            searchResults = new[]
            {
                new BasicSearchResult
                {
                    ExporterName = "test",
                    Id = new Guid("211C1DE8-906C-49DB-835C-4C2B78673B68"),
                    NotificationNumber = "GB 0001 000525",
                    WasteType = "RDF"
                },
                new BasicSearchResult
                {
                    ExporterName = "test2",
                    Id = new Guid("ECCAA419-B2D5-4B4B-9E45-57218D2C4C69"),
                    NotificationNumber = "GB 0001 000530",
                    WasteType = "Other"
                }
            };

            A.CallTo(() => mediator.SendAsync(A<SearchExportNotifications>.Ignored)).Returns(searchResults);
        }

        [Fact]
        public async Task GetIndex_ReturnsNewViewModel()
        {
            var result = await controller.Index() as ViewResult;

            Assert.IsType<BasicSearchViewModel>(result.Model);

            var model = result.Model as BasicSearchViewModel;

            Assert.Null(model.ExportSearchResults);
            Assert.False(model.HasSearched);
        }

        [Fact]
        public async Task PostIndex_SendsRequest()
        {
            var result = await controller.Index(postModel);

            A.CallTo(() => mediator.SendAsync(A<SearchExportNotifications>.Ignored)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async Task PostIndex_SetsModelSearchResults()
        {
            var result = await controller.Index(postModel) as ViewResult;

            Assert.Equal(searchResults, ((BasicSearchViewModel)result.Model).ExportSearchResults, new BasicSearchResultComparer());
        }

        [Fact]
        public async Task PostIndex_SetsHasSearchedToTrue()
        {
            var result = await controller.Index(postModel) as ViewResult;

            Assert.True(((BasicSearchViewModel)result.Model).HasSearched);
        }

        [Fact]
        public async Task PostIndex_SearchReturnsNullReturnsViewmodel()
        {
            A.CallTo(() => mediator.SendAsync(A<SearchExportNotifications>.Ignored)).Returns<IList<BasicSearchResult>>(null);

            var result = await controller.Index(postModel) as ViewResult;

            var model = result.Model as BasicSearchViewModel;

            Assert.Null(model.ExportSearchResults);
        }

        private class BasicSearchResultComparer : IEqualityComparer<BasicSearchResult>
        {
            public bool Equals(BasicSearchResult x, BasicSearchResult y)
            {
                return x.Id == y.Id
                       && x.ExporterName == y.ExporterName
                       && x.NotificationNumber == y.NotificationNumber
                       && x.WasteType == y.WasteType;
            }

            public int GetHashCode(BasicSearchResult obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}