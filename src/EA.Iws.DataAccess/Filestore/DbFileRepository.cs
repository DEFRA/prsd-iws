namespace EA.Iws.DataAccess.Filestore
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.FileStore;

    internal class DbFileRepository : IFileRepository
    {
        private readonly IwsFileStoreContext fileStoreContext;

        public DbFileRepository(IwsFileStoreContext fileStoreContext)
        {
            this.fileStoreContext = fileStoreContext;
        }

        public async Task<Guid> Store(File file)
        {
            fileStoreContext.Files.Add(file);

            await fileStoreContext.SaveChangesAsync();

            return file.Id;
        }

        public async Task<File> Get(Guid id)
        {
            return await fileStoreContext.Files.SingleAsync(p => p.Id == id);
        }
    }
}