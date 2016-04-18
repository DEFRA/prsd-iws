namespace EA.Iws.Web.Tests.Unit.Infrastructure
{
    using System.IO;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FakeItEasy;
    using Web.Infrastructure;
    using Xunit;

    public class HtmlHelperExtensionsTests
    {
        private readonly HtmlHelper htmlHelper = CreateHtmlHelper<TestModel>();

        [Fact]
        public void MailtoLink_ReturnsCorrectValue()
        {
            var expected = new MvcHtmlString("<a href=\"mailto:test@test.com\">test@test.com</a>");

            Assert.Equal(expected.ToHtmlString(), htmlHelper.MailtoLink("test@test.com").ToHtmlString());
        }

        private static HtmlHelper<T> CreateHtmlHelper<T>() where T : new()
        {
            var viewDataDictionary = new ViewDataDictionary(new T());

            var controllerContext = new ControllerContext(A.Fake<HttpContextBase>(),
                new RouteData(),
                A.Fake<ControllerBase>());

            var viewContext = new ViewContext(controllerContext, A.Fake<IView>(), viewDataDictionary, new TempDataDictionary(), A.Fake<TextWriter>());

            var viewDataContainer = A.Fake<IViewDataContainer>();

            A.CallTo(() => viewDataContainer.ViewData).Returns(viewDataDictionary);

            return new HtmlHelper<T>(viewContext, viewDataContainer);
        }

        private class TestModel
        {
            public string TestProperty { get; set; }
        }
    }
}