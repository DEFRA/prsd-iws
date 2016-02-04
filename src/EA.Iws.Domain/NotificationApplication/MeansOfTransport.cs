namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Core.MeansOfTransport;
    using Prsd.Core;
    using Prsd.Core.Domain;
    using Prsd.Core.Helpers;

    public class MeansOfTransport : Entity
    {
        private const char Separator = ';';

        protected string MeansOfTransportInternal { get; set; }

        public Guid NotificationId { get; private set; }

        protected MeansOfTransport()
        {
        }

        public MeansOfTransport(Guid notificationId)
        {
            NotificationId = notificationId;
        }

        public IOrderedEnumerable<TransportMethod> Route
        {
            get
            {
                if (string.IsNullOrWhiteSpace(MeansOfTransportInternal))
                {
                    return new TransportMethod[] { }.OrderBy(m => m);
                }

                //OrderBy with a key of 0 returns the elements in their original order.
                return MeansOfTransportInternal
                    .Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(MeansOfTransportHelper.GetTransportMethodFromToken)
                    .OrderBy(transport => 0);

                throw new NotImplementedException();
            }
        }

        public void SetRoute(IList<TransportMethod> meansOfTransport)
        {
            Guard.ArgumentNotNull(() => meansOfTransport, meansOfTransport);

            if (meansOfTransport.Count == 0)
            {
                throw new InvalidOperationException(
                    "Attempted to set means of transport to an empty list. Notification: " 
                    + this.NotificationId);
            }

            var builder = new StringBuilder();
            TransportMethod previousMeans = default(TransportMethod);

            for (int i = 0; i < meansOfTransport.Count; i++)
            {
                if (previousMeans == meansOfTransport[i])
                {
                    throw new InvalidOperationException(
                        "Attempted to set means of transport with two consecutive means of transport being the same. The sequence was: "
                        + string.Join(string.Empty, meansOfTransport.Select(mot => EnumHelper.GetShortName(mot))));
                }

                builder.Append(EnumHelper.GetShortName(meansOfTransport[i]));

                if (i < meansOfTransport.Count - 1)
                {
                    builder.Append(Separator);
                }

                previousMeans = meansOfTransport[i];
            }

            this.MeansOfTransportInternal = builder.ToString();
        }        
    }
}