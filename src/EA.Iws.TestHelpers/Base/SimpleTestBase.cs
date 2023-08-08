namespace EA.Iws.TestHelpers.Base
{
    using System.Globalization;
    using System.Threading;

    public class SimpleTestBase
    {
        public SimpleTestBase()
        {
            CultureInfo ci = new CultureInfo("en-GB");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}
