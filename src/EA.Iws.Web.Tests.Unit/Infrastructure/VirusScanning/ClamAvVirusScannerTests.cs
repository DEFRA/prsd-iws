namespace EA.Iws.Web.Tests.Unit.Infrastructure.VirusScanning
{
    using System;
    using System.Collections.Generic;
    using FakeItEasy;
    using nClam;
    using Web.Infrastructure.VirusScanning;
    using Xunit;

    public class ClamAvVirusScannerTests
    {
        private readonly IClamClientWrapper clamClientWrapper;
        private readonly ClamAvVirusScanner clamAvVirusScanner;

        public ClamAvVirusScannerTests()
        {
            clamClientWrapper = A.Fake<IClamClientWrapper>();

            clamAvVirusScanner = new ClamAvVirusScanner(clamClientWrapper);
        }

        [Fact]
        public void ScanFile_ShouldThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => clamAvVirusScanner.ScanFile(A.Dummy<byte[]>()));
        }

        [Fact]
        public async void ScanFileAsync_GivenCleanResult_CleanResultShouldBeReturned()
        {
            var file = A.Dummy<byte[]>();

            A.CallTo(() => clamClientWrapper.ScanFileAsync(file)).Returns(new ClamScanResult("ok"));

            var result = await clamAvVirusScanner.ScanFileAsync(file);

            Assert.Equal(ScanResult.Clean, result);
        }

        [Fact]
        public async void ScanFileAsync_GivenVirusResult_VirusResultShouldBeReturned()
        {
            var file = A.Dummy<byte[]>();

            A.CallTo(() => clamClientWrapper.ScanFileAsync(file)).Returns(new ClamScanResult("found"));

            var result = await clamAvVirusScanner.ScanFileAsync(file);

            Assert.Equal(ScanResult.Virus, result);
        }

        [Fact]
        public async void ScanFileAsync_GivenErrorResult_ErrorResultShouldBeReturned()
        {
            var file = A.Dummy<byte[]>();

            A.CallTo(() => clamClientWrapper.ScanFileAsync(file)).Returns(new ClamScanResult("error"));

            var result = await clamAvVirusScanner.ScanFileAsync(file);

            Assert.Equal(ScanResult.Error, result);
        }

        [Fact]
        public async void ScanFileAsync_GivenUnknownResult_UnknownResultShouldBeReturned()
        {
            var file = A.Dummy<byte[]>();

            A.CallTo(() => clamClientWrapper.ScanFileAsync(file)).Returns(new ClamScanResult("unknown"));

            var result = await clamAvVirusScanner.ScanFileAsync(file);

            Assert.Equal(ScanResult.Unknown, result);
        }
    }
}