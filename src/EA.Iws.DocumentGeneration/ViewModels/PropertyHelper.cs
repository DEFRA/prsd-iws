namespace EA.Iws.DocumentGeneration.ViewModels
{
    using System;
    using System.Reflection;

    internal class PropertyHelper
    {
        public static PropertyInfo[] GetPropertiesForViewModel(Type viewModelType)
        {
            return viewModelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
