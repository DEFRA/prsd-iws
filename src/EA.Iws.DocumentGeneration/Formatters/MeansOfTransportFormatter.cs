namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Linq;
    using Core.MeansOfTransport;
    using Prsd.Core.Helpers;

    public class MeansOfTransportFormatter
    {
        public string MeansOfTransportAsString(IOrderedEnumerable<TransportMethod> meansOfTransport)
        {
            if (meansOfTransport == null || !meansOfTransport.Any())
            {
                return string.Empty;
            }

            return string.Join("-", meansOfTransport.Select(m => EnumHelper.GetShortName(m)));
        }
    }
}