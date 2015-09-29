namespace EA.Iws.Core.Shared
{
    using System.ComponentModel;

    public enum ProvidedBy
    {
        [Description("I will provide this")]
        Notifier = 1,

        [Description("My importer-consignee will provide it directly to you")]
        Importer = 2
    }
}
