namespace EA.Iws.Domain.FileStore
{
    using System;
    using System.Threading.Tasks;

    public interface IFileRepository
    {
        Task<Guid> Store(File file);

        Task<File> Get(Guid id);

        Task Remove(Guid id);
    }
}