namespace EA.Iws.Web.Tests.Unit.Controllers.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Api.Client;
    using Areas.Admin.Controllers;
    using Areas.Admin.ViewModels;
    using Core.Admin.Search;
    using FakeItEasy;
    using Requests.Admin;
    using Xunit;

    public class HomeControllerTests
    {
        private readonly HomeController controller;
        private readonly IIwsClient client;
        private readonly BasicSearchViewModel postModel;
        private readonly IList<BasicSearchResult> searchResults; 

        public HomeControllerTests()
        {
            client = A.Fake<IIwsClient>();

            controller = new HomeController(() => client);

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

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetBasicSearchResults>.Ignored))
                .Returns(searchResults);
        }

        [Fact]
        public void GetIndex_ReturnsNewViewModel()
        {
            var result = controller.Index() as ViewResult;

            Assert.IsType<BasicSearchViewModel>(result.Model);

            var model = result.Model as BasicSearchViewModel;

            Assert.Null(model.SearchResults);
            Assert.False(model.HasSearched);
        }

        [Fact]
        public async Task PostIndex_SendsRequest()
        {
            var result = await controller.Index(postModel);

            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetBasicSearchResults>.Ignored))
                .MustHaveHappened(Repeated.Exactly.Once);
        }

        [Fact]
        public async Task PostIndex_SetsModelSearchResults()
        {
            var result = await controller.Index(postModel) as ViewResult;

            Assert.Equal(searchResults, ((BasicSearchViewModel)result.Model).SearchResults, new BasicSearchResultComparer());
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
            A.CallTo(() => client.SendAsync(A<string>.Ignored, A<GetBasicSearchResults>.Ignored))
                .Returns<IList<BasicSearchResult>>(null);

            var result = await controller.Index(postModel) as ViewResult;

            var model = result.Model as BasicSearchViewModel;

            Assert.Null(model.SearchResults);
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
