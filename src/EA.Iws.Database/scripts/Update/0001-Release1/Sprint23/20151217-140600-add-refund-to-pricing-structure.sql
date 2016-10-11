ALTER TABLE [Lookup].[PricingStructure]
ADD [PotentialRefund] MONEY NULL;
GO

CREATE PROCEDURE [#UpdatePricingStructure]
			@shipmentsFrom		INT, 
			@shipmentsTo		INT,
			@importExport		NVARCHAR(64),
			@recoveryDisposal	NVARCHAR(64),
			@competentAuthority	NVARCHAR(64),
			@isInterim			BIT,
			@price				MONEY,
			@potentialRefund	MONEY
AS
BEGIN
	DECLARE @shipmentQuantityRangeId UNIQUEIDENTIFIER;
	DECLARE @activityId UNIQUEIDENTIFIER;
	DECLARE @competentAuthorityId INT;
	DECLARE @pricingStructureId UNIQUEIDENTIFIER;

	SELECT @shipmentQuantityRangeId = [Id]
	FROM [Lookup].[ShipmentQuantityRange]
	WHERE (@shipmentsFrom IS NULL OR RangeFrom = @shipmentsFrom)
	AND (@shipmentsTo IS NULL OR RangeTo = @shipmentsTo);

	IF (@shipmentQuantityRangeId IS NULL)
	BEGIN
		RETURN -1;
	END;

	SELECT @activityId = [Id]
	FROM [Lookup].[Activity]
	WHERE TradeDirection = CASE WHEN @importExport = 'Export' THEN 1 WHEN @importExport = 'Import' THEN 2 ELSE 0 END
	AND NotificationType = CASE WHEN @recoveryDisposal = 'Recovery' THEN 1 WHEN @recoveryDisposal = 'Disposal'  THEN 2 ELSE 0 END
	AND IsInterim = @isInterim;

	IF (@activityId IS NULL)
	BEGIN
		RETURN -2;
	END;

	SELECT @pricingStructureId = [Id]
	FROM [Lookup].[PricingStructure]
	WHERE CompetentAuthority = CASE WHEN @competentAuthority = 'EA' THEN 1 WHEN @competentAuthority = 'SEPA' THEN 2 WHEN @competentAuthority = 'NIEA' THEN 3 WHEN @competentAuthority = 'NRW' THEN 4 ELSE 0 END
	AND ShipmentQuantityRangeId = @shipmentQuantityRangeId
	AND ActivityId = @activityId;

	IF (@pricingStructureId IS NULL)
	BEGIN
		RETURN -3;
	END;

	IF (@price IS NOT NULL)
	BEGIN
		UPDATE [Lookup].[PricingStructure]
		SET [Price] = @price
		WHERE [Id] = @pricingStructureId
	END;

	IF (@potentialRefund IS NOT NULL)
	BEGIN
		UPDATE [Lookup].[PricingStructure]
		SET [PotentialRefund] = @potentialRefund
		WHERE [Id] = @pricingStructureId
	END;

	RETURN 0;
END

GO

-- EA
-- 1 shipment
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'EA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'EA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'EA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'EA', 1, NULL, 0

-- 2 to 5
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'EA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'EA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'EA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'EA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'EA', 1, NULL, 0

-- 6 to 20
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'EA', 0, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'EA', 1, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'EA', 0, NULL, 1760
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'EA', 1, NULL, 1600
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'EA', 0, NULL, 1450
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'EA', 1, NULL, 1380
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'EA', 0, NULL, 1790
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'EA', 1, NULL, 1600

-- 21 to 100
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'EA', 0, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'EA', 1, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'EA', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'EA', 1, NULL, 4300
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'EA', 0, NULL, 3650
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'EA', 1, NULL, 4050
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'EA', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'EA', 1, NULL, 4300

-- 101 to 500
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'EA', 0, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'EA', 1, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'EA', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'EA', 1, NULL, 11200
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'EA', 0, NULL, 9350
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'EA', 1, NULL, 11450
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'EA', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'EA', 1, NULL, 11200

-- 501+
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'EA', 0, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'EA', 1, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'EA', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'EA', 1, NULL, 22300
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'EA', 0, NULL, 18250
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'EA', 1, NULL, 22550
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'EA', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'EA', 1, NULL, 22300
GO

-- SEPA
-- 1 shipment
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'SEPA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'SEPA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'SEPA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'SEPA', 1, NULL, 0

-- 2 to 5
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'SEPA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'SEPA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'SEPA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'SEPA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'SEPA', 1, NULL, 0

-- 6 to 20
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'SEPA', 0, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'SEPA', 1, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'SEPA', 0, NULL, 1760
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'SEPA', 1, NULL, 1600
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'SEPA', 0, NULL, 1450
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'SEPA', 1, NULL, 1380
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'SEPA', 0, NULL, 1790
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'SEPA', 1, NULL, 1600

-- 21 to 100
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'SEPA', 0, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'SEPA', 1, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'SEPA', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'SEPA', 1, NULL, 4300
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'SEPA', 0, NULL, 3650
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'SEPA', 1, NULL, 4050
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'SEPA', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'SEPA', 1, NULL, 4300

-- 101 to 500
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'SEPA', 0, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'SEPA', 1, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'SEPA', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'SEPA', 1, NULL, 11200
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'SEPA', 0, NULL, 9350
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'SEPA', 1, NULL, 11450
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'SEPA', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'SEPA', 1, NULL, 11200

-- 501+
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'SEPA', 0, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'SEPA', 1, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'SEPA', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'SEPA', 1, NULL, 22300
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'SEPA', 0, NULL, 18250
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'SEPA', 1, NULL, 22550
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'SEPA', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'SEPA', 1, NULL, 22300
GO

-- NIEA
-- 1 shipment
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'NIEA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'NIEA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'NIEA', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'NIEA', 1, NULL, 0

-- 2 to 5
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'NIEA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'NIEA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'NIEA', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'NIEA', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'NIEA', 1, NULL, 0

-- 6 to 20
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'NIEA', 0, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'NIEA', 1, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'NIEA', 0, NULL, 1760
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'NIEA', 1, NULL, 1600
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'NIEA', 0, NULL, 1450
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'NIEA', 1, NULL, 1380
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'NIEA', 0, NULL, 1790
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'NIEA', 1, NULL, 1600

-- 21 to 100
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'NIEA', 0, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'NIEA', 1, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'NIEA', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'NIEA', 1, NULL, 4300
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'NIEA', 0, NULL, 3650
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'NIEA', 1, NULL, 4050
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'NIEA', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'NIEA', 1, NULL, 4300

-- 101 to 500
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'NIEA', 0, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'NIEA', 1, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'NIEA', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'NIEA', 1, NULL, 11200
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'NIEA', 0, NULL, 9350
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'NIEA', 1, NULL, 11450
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'NIEA', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'NIEA', 1, NULL, 11200

-- 501+
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'NIEA', 0, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'NIEA', 1, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'NIEA', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'NIEA', 1, NULL, 22300
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'NIEA', 0, NULL, 18250
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'NIEA', 1, NULL, 22550
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'NIEA', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'NIEA', 1, NULL, 22300
GO

-- NRW
-- 1 shipment
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Recovery', 'NRW', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Export', 'Disposal', 'NRW', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Recovery', 'NRW', 1, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 1, 1, 'Import', 'Disposal', 'NRW', 1, NULL, 0

-- 2 to 5
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Recovery', 'NRW', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Export', 'Disposal', 'NRW', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Recovery', 'NRW', 1, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'NRW', 0, NULL, 0
EXEC #UpdatePricingStructure 2, 5, 'Import', 'Disposal', 'NRW', 1, NULL, 0

-- 6 to 20
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'NRW', 0, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'NRW', 1, NULL, 1250
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'NRW', 0, NULL, 1760
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'NRW', 1, NULL, 1600
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'NRW', 0, NULL, 1450
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'NRW', 1, NULL, 1380
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'NRW', 0, NULL, 1790
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'NRW', 1, NULL, 1600

-- 21 to 100
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'NRW', 0, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'NRW', 1, NULL, 2620
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'NRW', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'NRW', 1, NULL, 4300
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'NRW', 0, NULL, 3650
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'NRW', 1, NULL, 4050
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'NRW', 0, NULL, 3960
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'NRW', 1, NULL, 4300

-- 101 to 500
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'NRW', 0, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'NRW', 1, NULL, 6470
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'NRW', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'NRW', 1, NULL, 11200
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'NRW', 0, NULL, 9350
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'NRW', 1, NULL, 11450
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'NRW', 0, NULL, 9060
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'NRW', 1, NULL, 11200

-- 501+
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'NRW', 0, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'NRW', 1, NULL, 12930
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'NRW', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'NRW', 1, NULL, 22300
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'NRW', 0, NULL, 18250
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'NRW', 1, NULL, 22550
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'NRW', 0, NULL, 17960
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'NRW', 1, NULL, 22300
GO

ALTER TABLE [Lookup].[PricingStructure]
ALTER COLUMN [PotentialRefund] MONEY NOT NULL;
GO

DROP PROCEDURE [#UpdatePricingStructure]
GO