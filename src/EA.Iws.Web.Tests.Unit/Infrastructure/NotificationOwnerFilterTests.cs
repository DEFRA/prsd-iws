namespace EA.Iws.Web.Tests.Unit.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using FakeItEasy;
    using Prsd.Core.Mediator;
    using Requests.Notification;
    using Web.Infrastructure;
    using Xunit;

    public class NotificationOwnerFilterTests
    {
        private readonly NotificationOwnerFilter filter;
        private readonly IMediator mediator;
        private readonly ActionExecutingContext actionExecutingContext;
        private readonly Controller controller;

        public NotificationOwnerFilterTests()
        {
            mediator = A.Fake<IMediator>();
            A.CallTo(
                () =>
                    mediator.SendAsync(A<CheckIfNotificationOwner>.Ignored))
                .Returns(true);

            var request = A.Fake<HttpRequestBase>();
            var context = A.Fake<HttpContextBase>();

            A.CallTo(() => request.Url).Returns(new Uri("https://test.com"));
            A.CallTo(() => request.UrlReferrer).Returns(new Uri("https://test.com"));
            A.CallTo(() => context.Request).Returns(request);
            
            var dictionaryValueProv = new DictionaryValueProvider<object>(
            new Dictionary<string, object>() { { "id", Guid.NewGuid() } }, null);

            controller = A.Fake<Controller>();
            controller.ControllerContext = new ControllerContext(context, new RouteData(), controller);
            controller.ValueProvider = dictionaryValueProv;

            actionExecutingContext = new ActionExecutingContext
            {
                Controller = controller,
                Result = new HttpStatusCodeResult(HttpStatusCode.OK)
            };

            filter = new NotificationOwnerFilter { Mediator = mediator };
        }

        [Fact]
        public void CheckIfNotificationOwner()
        {
            A.CallTo(
                    () =>
                            mediator.SendAsync(A<CheckIfNotificationOwner>.Ignored))
                .Returns(true);

            filter.OnActionExecuting(actionExecutingContext);

            var result = (HttpStatusCodeResult)actionExecutingContext.Result;

            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public void NotOwnerReturnsForbidden()
        {
            A.CallTo(
                    () =>
                            mediator.SendAsync(A<CheckIfNotificationOwner>.Ignored))
                .Returns(false);

            filter.OnActionExecuting(actionExecutingContext);

            var result = (HttpStatusCodeResult)actionExecutingContext.Result;

            Assert.Equal((int)HttpStatusCode.Forbidden, result.StatusCode);
        }

        [Fact]
        public void MissingNotificationIdReturnsNotFound()
        {
            // Set a new value provider containing an non GUID value so that parsing fails.
            var newDictionaryValueProv = new DictionaryValueProvider<object>(
                new Dictionary<string, object>() { { "id", new { } } }, null);

            controller.ValueProvider = newDictionaryValueProv;

            filter.OnActionExecuting(actionExecutingContext);

            var result = (HttpStatusCodeResult)actionExecutingContext.Result;

            Assert.Equal((int)HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
