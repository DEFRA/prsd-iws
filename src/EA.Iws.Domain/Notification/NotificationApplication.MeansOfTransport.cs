namespace EA.Iws.Domain.Notification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core.MeansOfTransport;
    using Prsd.Core;

    public partial class NotificationApplication
    {
        public void SetMeansOfTransport(IList<MeansOfTransport> meansOfTransport)
        {
            Guard.ArgumentNotNull(() => meansOfTransport, meansOfTransport);

            if (meansOfTransport.Count == 0)
            {
                throw new InvalidOperationException("Attempted to set means of transport to an empty list. Use the delete means of transport method instead. Notification: " + this.Id);
            }

            var builder = new StringBuilder();
            MeansOfTransport previousMeans = null;

            for (int i = 0; i < meansOfTransport.Count; i++)
            {
                if (previousMeans != null && previousMeans.Symbol.Equals(meansOfTransport[i].Symbol))
                {
                    throw new InvalidOperationException("Attempted to set means of transport with two means of transport being the same. The sequence was: " 
                        + string.Join(string.Empty, meansOfTransport.Select(mot => mot.Symbol)));
                }

                builder.Append(meansOfTransport[i].Symbol);

                if (i < meansOfTransport.Count - 1)
                {
                    builder.Append(Core.MeansOfTransport.MeansOfTransport.Separator);
                }

                previousMeans = meansOfTransport[i];
            }

            this.MeansOfTransportInternal = builder.ToString();
        }
    }
}