namespace EA.Iws.Domain
{
    public interface INotificationPreviewWatermarkGenerator
    {
        byte[] GenerateNotificationPreviewWatermark(byte[] bytes);
    }
}
