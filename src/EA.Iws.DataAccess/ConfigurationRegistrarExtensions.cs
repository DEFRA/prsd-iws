namespace EA.Iws.DataAccess
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Linq;
    using System.Reflection;

    internal static class ConfigurationRegistrarExtensions
    {
        public static void AddFromNamespace(this ConfigurationRegistrar configurationRegistrar, string namespaceName)
        {
            var typesToRegister = Assembly.GetAssembly(typeof(ContextBase)).GetTypes()
              .Where(type => type.Namespace != null
                && type.Namespace.Equals(namespaceName))
              .Where(type => type.BaseType.IsGenericType
                && (type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>) 
                    || type.BaseType.GetGenericTypeDefinition() == typeof(ComplexTypeConfiguration<>)));

            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                configurationRegistrar.Add(configurationInstance);
            }
        }
    }
}