namespace EA.Iws.EmailMessaging
{
    using System.Threading.Tasks;

    internal interface IMessageHandler<in TMessage>
    {
        Task SendAsync(TMessage message);
    }
}
