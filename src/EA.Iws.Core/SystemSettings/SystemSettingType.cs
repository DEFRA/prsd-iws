namespace EA.Iws.Core.SystemSettings
{
    public enum SystemSettingType
    {
        EaChargeMatrixValidFrom = 1, //New EA charge matrix ValidFrom date
        SepaChargeMatrixValidFrom = 2, //New SEPA charge matrix ValidFrom date
        SepaFeeForNotSelfEnteringData = 3, //SEPA Additional Charge per shipment for not self entering data
        EaCustomImportAdditionalCharge = 4 //EA Import Disposal/Recovery custom per 100 (or part of) shipments over 1000 additional charge
    }
}
