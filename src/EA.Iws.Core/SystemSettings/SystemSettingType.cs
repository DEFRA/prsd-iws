namespace EA.Iws.Core.SystemSettings
{
    public enum SystemSettingType
    {
        //EA
        EaAdditionalChargeFixedFee = 1, // EA Fixed Additional charge for each data change
        EaCustomImportAdditionalCharge = 2, //EA Import Disposal/Recovery custom per 100 (or part of) shipments over 1000 additional charge
        EaCustomExportRecoveryAdditionalCharge = 3, //EA Export Recovery custom per 100 (or part of) shipments over 1000 additional charge
        EaCustomExportDisposalAdditionalCharge = 4, //EA Export Disposal custom per 100 (or part of) shipments over 1000 additional charge

        //SEPA
        SepaAdditionalChargeFixedFee = 5, //Sepa fixed additional charge for each data change
        SepaFeeForNotSelfEnteringData = 6 //SEPA Additional Charge per shipment for not self entering data
    }
}
