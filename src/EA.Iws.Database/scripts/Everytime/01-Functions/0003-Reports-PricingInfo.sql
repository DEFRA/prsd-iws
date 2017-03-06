IF OBJECT_ID('[Reports].[PricingInfo]') IS NULL
    EXEC('CREATE FUNCTION [Reports].[PricingInfo]() RETURNS @PricingInfo TABLE (Price MONEY NULL, PotentialRefund MONEY NULL) AS BEGIN RETURN END;')
GO	

ALTER FUNCTION [Reports].[PricingInfo](
    @notificationId		UNIQUEIDENTIFIER)
RETURNS @PricingInfo TABLE
(
    Price MONEY NULL,
    PotentialRefund MONEY NULL
)
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
        @isInterim = CASE WHEN IsInterim IS NOT NULL THEN IsInterim ELSE CASE WHEN NumberOfFacilities > 1 THEN 1 ELSE 0 END END
    FROM (
        SELECT
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType,
            1 AS TradeDirection,
            FC.IsInterim,
            COUNT(F.Id) AS NumberOfFacilities
        FROM 
            [Notification].[Notification] N
            INNER JOIN [Notification].[ShipmentInfo] S ON S.NotificationId = N.Id
            INNER JOIN [Notification].[FacilityCollection] FC ON FC.NotificationId = N.Id
            INNER JOIN [Notification].[Facility] F ON F.FacilityCollectionId = FC.Id
        WHERE 
            N.Id = @notificationId
        GROUP BY 
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType,
            FC.IsInterim

        UNION

        SELECT
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType,
            2 AS TradeDirection,
            I.IsInterim,
            COUNT(F.Id) AS NumberOfFacilities
        FROM 
            [ImportNotification].[Notification] N
            INNER JOIN [ImportNotification].[Shipment] S ON S.ImportNotificationId = N.Id
            INNER JOIN [ImportNotification].[FacilityCollection] FC ON FC.ImportNotificationId = N.Id
            INNER JOIN [ImportNotification].[Facility] F ON F.FacilityCollectionId = FC.Id
            LEFT JOIN [ImportNotification].[InterimStatus] I ON I.ImportNotificationId = N.Id
        WHERE 
            N.Id = @notificationId
        GROUP BY 
            N.Id,
            S.NumberOfShipments,
            N.CompetentAuthority,
            N.NotificationType,
            I.IsInterim) AS DATA;

    DECLARE @numberOfShipmentsMade INT;
    SELECT  @numberOfShipmentsMade = COUNT(MovementId) FROM [Reports].[Movements] WHERE NotificationId = @notificationId;

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
    DECLARE @potentialRefund MONEY;

    SELECT
        @price = [Price],
        @potentialRefund = CASE WHEN @numberOfShipmentsMade > 0 THEN 0 ELSE [PotentialRefund] END
    FROM
        [Lookup].[PricingStructure]
    WHERE
        CompetentAuthority = @competentAuthority
        AND ShipmentQuantityRangeId = @shipmentQuantityRangeId
        AND ActivityId = @activityId;

    INSERT @PricingInfo
    SELECT @price, @potentialRefund;

    RETURN;
END;
GO