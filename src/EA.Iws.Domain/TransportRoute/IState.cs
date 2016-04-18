namespace EA.Iws.Domain.TransportRoute
{
    public interface IState
    {
        Country Country { get; }

        CompetentAuthority CompetentAuthority { get; }
    }
}