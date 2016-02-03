namespace EA.Iws.Core.MeansOfTransport
{
    using System;

    public static class MeansOfTransportHelper
    {
        public static TransportMethod GetTransportMethodFromToken(string token)
        {
            switch (token.ToUpperInvariant())
            {
                case "R":
                    return TransportMethod.Road;

                case "T":
                    return TransportMethod.Train;

                case "S":
                    return TransportMethod.Sea;

                case "A":
                    return TransportMethod.Air;

                case "W":
                    return TransportMethod.InlandWaterways;

                default:
                    throw new ArgumentException(string.Format("Invalid token supplied: {0}", token), "token");
            }
        }
    }
}