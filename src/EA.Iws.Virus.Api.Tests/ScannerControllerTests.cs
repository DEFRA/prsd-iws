namespace EA.Iws.Virus.Api.Tests
{
    using System;
    using Controllers;
    using FluentAssertions;
    using System.Web.Http;
    using System.Web.Http.Results;
    using FakeItEasy;
    using Scanning;
    using Xunit;

    public class ScannerControllerTests
    {
        private readonly ScannerController controller;
        private readonly IVirusScanner scanner;

        public ScannerControllerTests()
        {
            scanner = A.Fake<IVirusScanner>();

            controller = new ScannerController(scanner);
        }

        [Fact]
        public void Controller_ShouldHaveAuthorizeAttribute()
        {
            typeof(ScannerController).Should().BeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void Controller_GetScanActionShouldHaveAnonymousAttribute()
        {
            typeof(ScannerController).GetMethod("Scan", new Type[0]).Should().BeDecoratedWith<AllowAnonymousAttribute>();
        }

        [Fact]
        public void Controller_PostScanActionShouldNotHaveAnonymousAttribute()
        {
            typeof(ScannerController).GetMethod("Scan", new[]{ typeof(byte[]) }).Should().NotBeDecoratedWith<AuthorizeAttribute>();
        }

        [Fact]
        public void GetScan_ShouldReturnOkResponse()
        {
            var result = controller.Scan() as OkNegotiatedContentResult<string>;

            result.Should().NotBeNull();
        }

        [Fact]
        public async void PostScan_GivenFile_ScannerShouldBeCalled()
        {
            var file = A.Dummy<byte[]>();

            await controller.Scan(file);

            A.CallTo(() => scanner.ScanFileAsync(file, string.Empty)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void PostScan_GivenFile_ScannerResultShouldBeReturned()
        {
            var file = A.Dummy<byte[]>();
            var scanResult = A.Dummy<ScanResult>();

            A.CallTo(() => scanner.ScanFileAsync(file, string.Empty)).Returns(A.Dummy<ScanResult>());

            var result = await controller.Scan(file) as OkNegotiatedContentResult<ScanResult>;

            result.Should().NotBeNull();
            result.Content.Should().Be(scanResult);
        }
    }
}
