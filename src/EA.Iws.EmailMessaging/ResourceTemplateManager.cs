namespace EA.Iws.EmailMessaging
{
    using System;
    using System.Collections.Concurrent;
    using Prsd.Email;
    using RazorEngine.Templating;

    internal class ResourceTemplateManager : ITemplateManager
    {
        private readonly ConcurrentDictionary<ITemplateKey, ITemplateSource> dynamicTemplates =
            new ConcurrentDictionary<ITemplateKey, ITemplateSource>();

        private readonly ITemplateLoader templateLoader;

        public ResourceTemplateManager(ITemplateLoader templateLoader)
        {
            this.templateLoader = templateLoader;
        }

        public ITemplateSource Resolve(ITemplateKey key)
        {
            var template = templateLoader.LoadTemplate(key.Name);

            return new LoadedTemplateSource(template);
        }

        public ITemplateKey GetKey(string name, ResolveType resolveType, ITemplateKey context)
        {
            return new NameOnlyTemplateKey(name, resolveType, context);
        }

        public void AddDynamic(ITemplateKey key, ITemplateSource source)
        {
            dynamicTemplates.AddOrUpdate(key, source, (k, oldSource) =>
            {
                if (oldSource.Template != source.Template)
                {
                    throw new InvalidOperationException("The same key was already used for another template!");
                }
                return source;
            });
        }
    }
}