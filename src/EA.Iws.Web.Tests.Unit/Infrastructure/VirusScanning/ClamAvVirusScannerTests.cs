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

        [Theory]
        [MemberData(nameof(VirusScanResults))]
        public async void ScanFileAsync_GivenCleanResult_CleanResultShouldBeReturned(string clamAvError, ScanResult expectedResult)
        {
            var file = A.Dummy<byte[]>();

            A.CallTo(() => clamClientWrapper.ScanFileAsync(file)).Returns(new ClamScanResult(clamAvError));

            var result = await clamAvVirusScanner.ScanFileAsync(file);

            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> VirusScanResults()
        {
            yield return new object[] { "ok", ScanResult.Clean };
            yield return new object[] { "found", ScanResult.Virus };
            yield return new object[] { "error", ScanResult.Error };
            yield return new object[] { "unknown", ScanResult.Unknown };
        }
    }
}