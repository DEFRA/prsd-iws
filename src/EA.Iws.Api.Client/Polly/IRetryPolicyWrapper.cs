namespace EA.Iws.Api.Client.Polly
{
    using System;
    using System.Threading.Tasks;

    public interface IRetryPolicyWrapper
    {
        Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> action);
    }
}
