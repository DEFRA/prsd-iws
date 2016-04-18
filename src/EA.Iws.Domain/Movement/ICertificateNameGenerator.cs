namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;

    public interface ICertificateNameGenerator
    {
        Task<string> GetValue(Movement movement);
    }
}