namespace EA.Iws.Core.WasteType
{
    using System.ComponentModel;
    public enum WasteInformationType
    {
        [Description("Net calorific value (Megajoules per kg)")]
        NetCalorificValue = 1,
        [Description("Moisture content")]
        MoistureContent = 2,
        [Description("Ash content")]
        AshContent = 3,
        [Description("Heavy metals (Megajoules per kg)")]
        HeavyMetals = 4,
        [Description("Chlorine")]
        Chlorine = 5,
        [Description("Energy")]
        Energy = 6
    }
}