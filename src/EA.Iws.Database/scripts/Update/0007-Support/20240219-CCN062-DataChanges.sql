GO

PRINT N'Correcting incorrect refund amount';
-- Export Disposal IsInterim 0 Range 6-20
UPDATE [Lookup].[PricingStructure]
	SET PotentialRefund = '475.00'
	WHERE Id = '442C6967-7C77-45F2-95A7-7428E379012A'

PRINT N'Correcting Description for SEPA Additional Charge Lookup.SystemSettings'
UPDATE [Lookup].[SystemSettings]
	SET [Description] = 'SEPA Additional Charge per shipment for not self entering data'
	WHERE Id = 3

PRINT N'Add new SystemSetting as per emails';
INSERT INTO [Lookup].[SystemSettings] (Id, [Value], [Description])
	VALUES (4, '1207', 'EA Import Disposal/Recovery custom per 100 (or part of) shipments over 1000 additional charge')

PRINT N'Updating EA Charge structure as per emails';
-- Import Disposal IsInterim 0 Range 1-5
UPDATE [Lookup].[PricingStructure] SET Price = '3330.00', PotentialRefund = '00.00' WHERE ID = '6CF3966E-1FF0-49CB-A73D-D890DCDA6ABB'
-- Import Disposal IsInterim 0 Range 6-20
UPDATE [Lookup].[PricingStructure] SET Price = '3739.00', PotentialRefund = '409.00' WHERE ID = 'ECE0889F-ACA6-4F89-86BE-3C3123A7C0C4'
-- Import Disposal IsInterim 0 Range 21-100
UPDATE [Lookup].[PricingStructure] SET Price = '4663.00', PotentialRefund = '1333.00' WHERE ID = '8F68E94B-F077-4CE2-8506-854CE912C166'
-- Import Disposal IsInterim 0 Range 101-300
UPDATE [Lookup].[PricingStructure] SET Price = '6700.00', PotentialRefund = '3370.00' WHERE ID = '2F167B34-3A0C-4D2D-BD61-E5DC15A62615'
-- Import Disposal IsInterim 0 Range 301-500
UPDATE [Lookup].[PricingStructure] SET Price = '9660.00', PotentialRefund = '6330.00' WHERE ID = '8D8E2BEC-69CB-4AAD-9F5E-D1840CB28108'
-- Import Disposal IsInterim 0 Range 501-1000
UPDATE [Lookup].[PricingStructure] SET Price = '12065.00', PotentialRefund = '8735.00' WHERE ID = 'B8CB42E4-B04C-49DD-B074-8F1139E9D1D3'

-- Import Disposal IsInterim 1 Range 1-5
UPDATE [Lookup].[PricingStructure] SET Price = '3330.00', PotentialRefund = '00.00' WHERE ID = '08B0636E-B575-4C3D-8C50-4BB1BE65562D'
-- Import Disposal IsInterim 1 Range 6-20
UPDATE [Lookup].[PricingStructure] SET Price = '3739.00', PotentialRefund = '409.00' WHERE ID = '685A92A0-93AB-4F55-B969-D60737F9DCDE'
-- Import Disposal IsInterim 1 Range 21-100
UPDATE [Lookup].[PricingStructure] SET Price = '4663.00', PotentialRefund = '1333.00' WHERE ID = 'CF170991-DC3A-4200-8309-1C146AD112E2'
-- Import Disposal IsInterim 1 Range 101-300
UPDATE [Lookup].[PricingStructure] SET Price = '6700.00', PotentialRefund = '3370.00' WHERE ID = '09FF17BD-594D-4767-B7B1-E40973EEDD14'
-- Import Disposal IsInterim 1 Range 301-500
UPDATE [Lookup].[PricingStructure] SET Price = '9660.00', PotentialRefund = '6330.00' WHERE ID = 'EA8FAC87-53F0-428B-8DFD-319030906D43'
-- Import Disposal IsInterim 1 Range 501-1000
UPDATE [Lookup].[PricingStructure] SET Price = '12065.00', PotentialRefund = '8735.00' WHERE ID = 'F92251B4-06DD-4B0F-8415-8EB2F72AF511'