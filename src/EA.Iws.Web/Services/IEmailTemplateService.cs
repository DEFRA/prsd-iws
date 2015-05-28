namespace EA.Iws.Web.Services
{
    public interface IEmailTemplateService
    {
        EmailTemplate TemplateWithDynamicModel(string templateName, object model);
    }
}