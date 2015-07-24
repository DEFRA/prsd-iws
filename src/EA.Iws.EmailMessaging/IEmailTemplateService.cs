namespace EA.Iws.EmailMessaging
{
    public interface IEmailTemplateService
    {
        EmailTemplate TemplateWithDynamicModel(string templateName, object model);
    }
}