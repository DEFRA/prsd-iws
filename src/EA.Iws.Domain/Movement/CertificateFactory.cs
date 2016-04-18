namespace EA.Iws.Domain.Movement
{
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using FileStore;

    [AutoRegister]
    public class CertificateFactory
    {
        public async Task<File> CreateForMovement(ICertificateNameGenerator nameGenerator, 
            Movement movement, 
            byte[] content, 
            string fileType)
        {
            var fileName = await nameGenerator.GetValue(movement);
            return new File(fileName, fileType, content);
        }
    }
}