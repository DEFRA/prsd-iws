namespace EA.Iws.Scanning.Tests.Unit
{
    using System;
    using System.IO;
    using FakeItEasy;
    using Xunit;

    public class WriteFileVirusScannerTests
    {
        private readonly WriteFileVirusScanner virusScanner;
        private readonly IFileAccess fileAccess;
        private readonly string FileName = "File.txt";
        private const int Timeout = 30000;

        public WriteFileVirusScannerTests()
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
           
            fileAccess = A.Fake<IFileAccess>();

            virusScanner = new WriteFileVirusScanner(Timeout, fileAccess, path);
            
            A.CallTo(() => fileAccess.GetTemporaryFileName(path)).Returns(FileName);
        }

        [Fact]
        public async void ScanFileAsync_GivenWriteFileThrowsException_IoExceptionShouldBeThrown()
        {
            var file = A.Dummy<byte[]>();

            A.CallTo(() => fileAccess.WriteFileAsync(A<string>._, file)).Throws<Exception>();

            var result = await Xunit.Record.ExceptionAsync(() => virusScanner.ScanFileAsync(file, A.Dummy<string>()));

            Assert.Equal(typeof(IOException), result.GetType());
            Assert.Equal("Virus scan could not write to file system", result.Message);
        }

        [Fact]
        public async void ScanFileAsync_GivenFileHasBeenRemoved_ScanResultShouldBeVirus()
        {
            A.CallTo(() => fileAccess.FileExists(FileName)).Returns(false);

            var result = await virusScanner.ScanFileAsync(A.Dummy<byte[]>(), A.Dummy<string>());

            Assert.Equal(ScanResult.Virus, result);
        }

        [Fact]
        public async void ScanFileAsync_GivenFileHasNotBeenRemoved_ScanResultShouldBeClean()
        {
            A.CallTo(() => fileAccess.FileExists(FileName)).Returns(true);

            var result = await virusScanner.ScanFileAsync(A.Dummy<byte[]>(), A.Dummy<string>());

            Assert.Equal(ScanResult.Clean, result);
        }

        [Fact]
        public async void ScanFileAsync_GivenFileHasNotBeenRemoved_FileShouldBeDeleted()
        {
            A.CallTo(() => fileAccess.FileExists(FileName)).Returns(true);

            var result = await virusScanner.ScanFileAsync(A.Dummy<byte[]>(), A.Dummy<string>());

            A.CallTo(() => fileAccess.DeleteFile(FileName)).MustHaveHappenedOnceExactly();
        }

        [Fact]
        public async void ScanFileAsync_GivenTemporaryFileCouldNotBeDeleted_ScanResultShouldBeClean()
        {
            A.CallTo(() => fileAccess.FileExists(FileName)).Returns(true);
            A.CallTo(() => fileAccess.DeleteFile(FileName)).Throws<IOException>();

            var result = await virusScanner.ScanFileAsync(A.Dummy<byte[]>(), A.Dummy<string>());

            Assert.Equal(ScanResult.Clean, result);
        }
    }
}
