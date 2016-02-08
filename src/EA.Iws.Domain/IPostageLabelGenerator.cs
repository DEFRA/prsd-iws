namespace EA.Iws.Domain
{
    public interface IPostageLabelGenerator
    {
        byte[] GeneratePostageLabel(Core.Notification.UKCompetentAuthority competentAuthority);
    }
}
