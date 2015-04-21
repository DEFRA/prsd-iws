namespace EA.Iws.Core.Cqrs
{
    using System.Threading.Tasks;

    public interface IResourceAuthorizationManager
    {
        Task<bool> CheckAccessAsync(ResourceAuthorizationContext context);
    }
}