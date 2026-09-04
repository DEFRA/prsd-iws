--SEPA Single ship price update
UPDATE [Lookup].[PricingFixedFee] SET [Price]=9884 WHERE Id='D39271D5-381C-43C0-A137-A190DB98F1AF' AND CompetentAuthority=2 AND WasteCategoryTypeId=11;

--SEPA Platform/rig price update
UPDATE [Lookup].[PricingFixedFee] SET [Price]=9884 WHERE Id='AA5D920C-2E15-4EA1-A68A-401DDD50D3CF' AND CompetentAuthority=2 AND WasteCategoryTypeId=12;

--SEPA Additional Charge per shipment for not self entering data price update
UPDATE [Lookup].[SystemSettings] SET [Value]=27 WHERE Id=3;

--SEPA fixed additional charge for each data changes price update
UPDATE [Lookup].[SystemSettings] SET [Value]=190 WHERE Id=6;