namespace EA.Iws.EmailMessaging
{
    using Autofac;
    using RazorEngine;
    using RazorEngine.Configuration;
    using RazorEngine.Templating;

    internal class RazorEngineSetup : IStartable
    {
        private readonly ITemplateManager templateManager;

        public RazorEngineSetup(ITemplateManager templateManager)
        {
            this.templateManager = templateManager;
        }

        public void Start()
        {
            var config = new TemplateServiceConfiguration();
            config.TemplateManager = templateManager;
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
        }
    }
}