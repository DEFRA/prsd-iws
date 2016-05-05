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
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'NIEA', 0, NULL, 935
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Recovery', 'NIEA', 1, NULL, 935
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'NIEA', 0, NULL, 935
EXEC #UpdatePricingStructure 6, 20, 'Export', 'Disposal', 'NIEA', 1, NULL, 935
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'NIEA', 0, NULL, 1085
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Recovery', 'NIEA', 1, NULL, 1085
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'NIEA', 0, NULL, 1085
EXEC #UpdatePricingStructure 6, 20, 'Import', 'Disposal', 'NIEA', 1, NULL, 1085

-- 21 to 100
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'NIEA', 0, NULL, 1960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Recovery', 'NIEA', 1, NULL, 1960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'NIEA', 0, NULL, 1960
EXEC #UpdatePricingStructure 21, 100, 'Export', 'Disposal', 'NIEA', 1, NULL, 1960
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'NIEA', 0, NULL, 2735
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Recovery', 'NIEA', 1, NULL, 2735
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'NIEA', 0, NULL, 2735
EXEC #UpdatePricingStructure 21, 100, 'Import', 'Disposal', 'NIEA', 1, NULL, 2735

-- 101 to 500
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'NIEA', 0, NULL, 4850
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Recovery', 'NIEA', 1, NULL, 4850
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'NIEA', 0, NULL, 4850
EXEC #UpdatePricingStructure 101, 500, 'Export', 'Disposal', 'NIEA', 1, NULL, 4850
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'NIEA', 0, NULL, 7010
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Recovery', 'NIEA', 1, NULL, 7010
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'NIEA', 0, NULL, 7010
EXEC #UpdatePricingStructure 101, 500, 'Import', 'Disposal', 'NIEA', 1, NULL, 7010

-- 501+
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'NIEA', 0, NULL, 9695
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Recovery', 'NIEA', 1, NULL, 9695
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'NIEA', 0, NULL, 9695
EXEC #UpdatePricingStructure 501, NULL, 'Export', 'Disposal', 'NIEA', 1, NULL, 9695
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'NIEA', 0, NULL, 13685
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Recovery', 'NIEA', 1, NULL, 13685
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'NIEA', 0, NULL, 13685
EXEC #UpdatePricingStructure 501, NULL, 'Import', 'Disposal', 'NIEA', 1, NULL, 13685
GO

DROP PROCEDURE [#UpdatePricingStructure]
GO