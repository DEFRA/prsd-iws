UPDATE
    [Lookup].[UnitedKingdomCompetentAuthority]
SET 
    [BacsBank] = 'Royal Bank of Scotland',
    [BacsBankAddress] = 'National Westminster Bank Plc, 2 ½ Devonshire Square, London EC2M 4BA',
    [BacsSortCode] = '60-70-80',
    [BacsAccountNumber] = '10014438',
    [BacsIban] = 'GB70NWBK60708010014438',
    [BacsSwiftBic] = 'NWBKGB2L',
    [RemittancePostalAddress] = 'Natural Resources Wales, Income Department, PO BOX 663, Cardiff, CF24 0TP'
WHERE
    [UnitedKingdomCountry] = 'Wales'