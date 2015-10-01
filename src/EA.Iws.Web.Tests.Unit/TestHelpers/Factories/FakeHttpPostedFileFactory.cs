namespace EA.Iws.Web.Tests.Unit.TestHelpers.Factories
{
    using System.IO;
    using System.Text;
    using System.Web;
    using FakeItEasy;

    public static class FakeHttpPostedFileFactory
    {
        public static HttpPostedFileBase CreateTestFile()
        {
            var postedFile = A.Fake<HttpPostedFileBase>();

            var str = "file content";
            var buffer = Encoding.UTF8.GetBytes(str);
            var stream = new MemoryStream(buffer);

            A.CallTo(() => postedFile.InputStream).Returns(stream);
            A.CallTo(() => postedFile.FileName).Returns("test.txt");
            return postedFile;
        }
    }
}