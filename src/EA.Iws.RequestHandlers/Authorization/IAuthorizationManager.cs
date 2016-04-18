namespace EA.Iws.RequestHandlers.Authorization
{
    using System.Threading.Tasks;

    public interface IAuthorizationManager
    {
        Task<bool> CheckAccessAsync(AuthorizationContext context);
    }
}
