IF OBJECT_ID('[Reports].[PotentialRefund]') IS NULL
    EXEC('CREATE FUNCTION [Reports].[PotentialRefund]() RETURNS MONEY AS BEGIN RETURN 0 END;')
GO	

ALTER FUNCTION [Reports].[PotentialRefund](
    @notificationId		UNIQUEIDENTIFIER)
RETURNS MONEY
AS
BEGIN

    DECLARE @numberOfShipments INT;
    DECLARE @competentAuthority INT;
    DECLARE @notificationType INT;
    DECLARE @tradeDirection INT;
    DECLARE @isInterim BIT;

    SELECT
        @numberOfShipments = NumberOfShipments,
        @competentAuthority = CompetentAuthority,
        @notificationType = NotificationType,
        @tradeDirection = TradeDirection,
        @isInterim = CASE WHEN NumberOfFacilities > 1 THEN 1 ELSE 0 END
    FROM (
        SELECT
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType,
            1 AS TradeDirection,
            COUNT(F.Id) AS NumberOfFacilities
        FROM 
            [Notification].[Notification] N
            INNER JOIN [Notification].[ShipmentInfo] S ON S.NotificationId = N.Id
            INNER JOIN [Notification].[Facility] F ON F.NotificationId = N.Id
        WHERE 
            N.Id = @notificationId
        GROUP BY 
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType

        UNION

        SELECT
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType,
            2 AS TradeDirection,
            COUNT(F.Id) AS NumberOfFacilities
        FROM 
            [ImportNotification].[Notification] N
            INNER JOIN [ImportNotification].[Shipment] S ON S.ImportNotificationId = N.Id
            INNER JOIN [ImportNotification].[FacilityCollection] FC ON FC.ImportNotificationId = N.Id
            INNER JOIN [ImportNotification].[Facility] F ON F.FacilityCollectionId = FC.Id
        WHERE 
            N.Id = @notificationId
        GROUP BY 
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType) AS DATA;

    DECLARE @activityId UNIQUEIDENTIFIER;
    DECLARE @shipmentQuantityRangeId UNIQUEIDENTIFIER;

    SELECT 
        @activityId = Id
    FROM 
        [Lookup].[Activity]
    WHERE
        TradeDirection = @tradeDirection
        AND NotificationType = @notificationType
        AND IsInterim = @isInterim;
        
    SELECT
        @shipmentQuantityRangeId = Id
    FROM
        [Lookup].[ShipmentQuantityRange]
    WHERE
         @numberOfShipments >= RangeFrom
         AND (RangeTo IS NULL OR @numberOfShipments <= RangeTo);

    DECLARE @price MONEY;

    SELECT
        @price = Price
    FROM
        [Lookup].[PricingStructure]
    WHERE
        CompetentAuthority = @competentAuthority
        AND ShipmentQuantityRangeId = @shipmentQuantityRangeId
        AND ActivityId = @activityId;

    RETURN @price;
END;
GO

