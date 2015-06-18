namespace EA.Iws.Core.MeansOfTransport
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Domain;

    public class MeansOfTransport : Enumeration
    {
        public const char Separator = ';';
        public static readonly MeansOfTransport Road = new MeansOfTransport(1, "Road", "R");
        public static readonly MeansOfTransport Train = new MeansOfTransport(2, "Train", "T");
        public static readonly MeansOfTransport Sea = new MeansOfTransport(3, "Sea", "S");
        public static readonly MeansOfTransport Air = new MeansOfTransport(4, "Air", "A");
        public static readonly MeansOfTransport InlandWaterways = new MeansOfTransport(5, "Inland Waterways", "W");

        public string Symbol { get; private set; }

        protected MeansOfTransport()
        {
        }

        private MeansOfTransport(int value, string displayName, string symbol)
            : base(value, displayName)
        {
            this.Symbol = symbol;
        }

        public static MeansOfTransport GetFromToken(string token)
        {
            Guard.ArgumentNotNullOrEmpty(() => token, token);

            switch (token.ToUpperInvariant())
            {
                case "R":
                    return Road;
                case "T":
                    return Train;
                case "S":
                    return Sea;
                case "A":
                    return Air;
                case "W":
                    return InlandWaterways;
                default:
                    throw new InvalidOperationException("Invalid token for a means of transport, token was: " + token);
            }
        }
    }
}
