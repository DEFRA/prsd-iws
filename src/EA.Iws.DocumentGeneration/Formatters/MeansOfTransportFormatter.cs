namespace EA.Iws.DocumentGeneration.Formatters
{
    using System.Linq;
    using Core.MeansOfTransport;

    public class MeansOfTransportFormatter
    {
        public string MeansOfTransportAsString(IOrderedEnumerable<MeansOfTransport> meansOfTransport)
        {
            if (meansOfTransport == null || !meansOfTransport.Any())
            {
                return string.Empty;
            }

            return string.Join(" - ", meansOfTransport.Select(m => m.Symbol));
        }
    }
}
