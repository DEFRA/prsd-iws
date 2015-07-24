namespace EA.Iws.EmailMessaging
{
    using System.IO;
    using RazorEngine;
    using RazorEngine.Templating;

    public class EmailTemplateService : IEmailTemplateService
    {
        private const string AssemblyName = "EA.Iws.EmailMessaging";
        private const string TemplateDirectory = "Templates";
        private const string HtmlTemplateExtension = "cshtml";
        private const string PlainTextTemplateExtension = "txt";

        private static Stream GetTemplate(string templateName, string extension)
        {
            var resourceName = string.Format("{0}.{1}.{2}.{3}", AssemblyName, TemplateDirectory, templateName, extension);

            return typeof(EmailTemplateService).Assembly.GetManifestResourceStream(resourceName);
        }

        private static string GetTemplateAsString(string templateName, string extension)
        {
            using (var stream = GetTemplate(templateName, extension))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public EmailTemplate TemplateWithDynamicModel(string templateName, object model)
        {
            string htmlTemplateKey = templateName;
            string plainTextTemplateKey = templateName + "PlainText";

            string htmlResult = RunTemplateWithDynamicModel(htmlTemplateKey, templateName, model, HtmlTemplateExtension);
            string plainTextResult = RunTemplateWithDynamicModel(plainTextTemplateKey, templateName, model, PlainTextTemplateExtension);
            
            return new EmailTemplate(htmlResult, plainTextResult);
        }

        private static string RunTemplateWithDynamicModel(string templateKey, string templateName, object model, string fileExtension)
        {
            bool isTemplated = Engine.Razor.IsTemplateCached(templateKey, null);
            string result;

            if (isTemplated)
            {
                result = Engine.Razor.Run(templateKey, null, model);
            }
            else
            {
                result = Engine.Razor.RunCompile(GetTemplateAsString(templateName, fileExtension), templateKey,
                    null, model);
            }

            return result;
        }
    }
}