IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'Notification')
	EXEC('CREATE SCHEMA [Notification] AUTHORIZATION [dbo]');
GO

IF OBJECT_ID('[Notification].[GetPricingInfo]') IS NULL
    EXEC('CREATE FUNCTION [Notification].[GetPricingInfo]() RETURNS @PricingInfo TABLE (Price MONEY NULL, PotentialRefund MONEY NULL) AS BEGIN RETURN END;')
GO

ALTER FUNCTION [Notification].[GetPricingInfo](
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
	DECLARE @submittedDate DATETIMEOFFSET;
	DECLARE @fixedWasteCategoryFee MONEY;
	
	SELECT @submittedDate = ChangeDate
	 FROM (
        SELECT nsc.ChangeDate as ChangeDate
        FROM 
            [Notification].[Notification] N
			LEFT JOIN [Notification].[NotificationAssessment] na on na.NotificationApplicationId = N.Id
			LEFT JOIN [Notification].[NotificationStatusChange] nsc on nsc.NotificationAssessmentId = na.Id
        WHERE N.Id = @notificationId
		AND nsc.[Status] = 2
		UNION
		SELECT
			nsc.ChangeDate
        FROM 
            [ImportNotification].[Notification] N
			LEFT JOIN [ImportNotification].[NotificationAssessment] na on na.NotificationApplicationId = N.Id
			LEFT JOIN [ImportNotification].[NotificationStatusChange] nsc on nsc.NotificationAssessmentId = na.Id
        WHERE N.Id = @notificationId
		AND nsc.NewStatus = 2
		) AS DATA;

	SELECT @submittedDate = CASE WHEN @submittedDate IS NULL THEN GETDATE() ELSE @submittedDate END

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
            LEFT JOIN [Notification].[FacilityCollection] FC ON FC.NotificationId = N.Id
            LEFT JOIN [Notification].[Facility] F ON F.FacilityCollectionId = FC.Id
        WHERE N.Id = @notificationId
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
            LEFT JOIN [ImportNotification].[FacilityCollection] FC ON FC.ImportNotificationId = N.Id
            LEFT JOIN [ImportNotification].[Facility] F ON F.FacilityCollectionId = FC.Id
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

    SELECT 
        @activityId = Id
    FROM 
        [Lookup].[Activity]
    WHERE
        TradeDirection = @tradeDirection
        AND NotificationType = @notificationType
        AND IsInterim = @isInterim;

    DECLARE @price MONEY;
    DECLARE @potentialRefund MONEY;

    SELECT TOP 1
        @price = ps.[Price],
        @potentialRefund = CASE WHEN @numberOfShipmentsMade > 0 THEN 0 ELSE ps.[PotentialRefund] END
    FROM
        [Lookup].[PricingStructure] ps
		LEFT JOIN [Lookup].[ShipmentQuantityRange] sqr ON sqr.Id = ps.ShipmentQuantityRangeId
    WHERE
        ps.CompetentAuthority = @competentAuthority
        AND ps.ActivityId = @activityId
		AND ps.ValidFrom <= @submittedDate
		AND (@numberOfShipments >= sqr.RangeFrom AND (sqr.RangeTo IS NULL OR @numberOfShipments <= sqr.RangeTo))
	ORDER BY ValidFrom desc;

	IF @competentAuthority = 1 AND GETDATE() >= (SELECT [Value] from [Lookup].[SystemSettings] where Id = 1) 
	BEGIN
		--Use the new fees and logic
		SELECT @fixedWasteCategoryFee = Price 
		FROM [Lookup].[PricingFixedFee]
		WHERE WasteCategoryTypeId in (
			SELECT nwt.WasteCategoryType FROM Notification.Notification n
			LEFT JOIN Notification.WasteType nwt on nwt.NotificationId = n.Id
			WHERE n.id = @notificationId
			UNION 
			SELECT inwt.WasteCategoryType FROM ImportNotification.Notification n
			LEFT JOIN ImportNotification.WasteType inwt on inwt.ImportNotificationId = n.Id
			WHERE n.id = @notificationId)
		AND CompetentAuthority = @competentAuthority

		IF @fixedWasteCategoryFee > 0 
		BEGIN
			INSERT @PricingInfo
			SELECT @fixedWasteCategoryFee, 0;
			RETURN;
		END;

		IF @numberOfShipments > 1000
		BEGIN
			DECLARE @hundreds INT
			SET @hundreds = (@numberOfShipments - 901) / 100
			SET @price += (@price * 0.10 * @hundreds)

			--Refund is the price minus the price for the lowest range
			--So rather than calculate the refund based on something like the logic above just subtract the price for lowest range from calculated price above
			SELECT TOP 1
				@potentialRefund = CASE WHEN @numberOfShipmentsMade > 0 THEN 0 ELSE (@price - ps.Price) END
			FROM
				[Lookup].[PricingStructure] ps
				LEFT JOIN [Lookup].[ShipmentQuantityRange] sqr ON sqr.Id = ps.ShipmentQuantityRangeId
			WHERE
				ps.CompetentAuthority = @competentAuthority
				AND ps.ActivityId = @activityId
				AND ps.ValidFrom <= @submittedDate
				AND (1 >= sqr.RangeFrom AND (sqr.RangeTo IS NULL OR 1 <= sqr.RangeTo))
			ORDER BY ValidFrom desc;
		END

		DECLARE @wasteComponentFees MONEY;
		SELECT @wasteComponentFees = SUM(Price) FROM [Lookup].[PricingFixedFee] where WasteComponentTypeId in (
			SELECT wc.WasteComponentType 
			FROM [Notification].[Notification] n
			LEFT JOIN [Notification].[WasteComponentInfo] wc on wc.NotificationId = n.Id
			WHERE n.id = @notificationId
			UNION 
			SELECT iwc.WasteComponentType 
			FROM [ImportNotification].[Notification] n
			LEFT JOIN ImportNotification.WasteComponent iwc on iwc.ImportNotificationId = n.Id
			WHERE n.id = @notificationId)

		SELECT @price += ISNULL(@wasteComponentFees,0);
	END;

	IF @competentAuthority = 2 AND GETDATE() >= (SELECT [Value] from [Lookup].[SystemSettings] where Id = 2) 
	BEGIN
		SELECT @fixedWasteCategoryFee = Price 
		FROM [Lookup].[PricingFixedFee]
		WHERE WasteCategoryTypeId in (
			SELECT nwt.WasteCategoryType 
			FROM [Notification].[Notification] n
			LEFT JOIN [Notification].[WasteType] nwt on nwt.NotificationId = n.Id
			WHERE n.id = @notificationId
			UNION 
			SELECT inwt.WasteCategoryType 
			FROM [ImportNotification].[Notification] n
			LEFT JOIN ImportNotification.WasteType inwt on inwt.ImportNotificationId = n.Id
			WHERE n.id = @notificationId)
		AND CompetentAuthority = @competentAuthority

		IF @fixedWasteCategoryFee > 0 
		BEGIN
			INSERT @PricingInfo
			SELECT @fixedWasteCategoryFee, 0;
			RETURN;
		END;

		DECLARE @applySelfEnterDataFee bit;
		SELECT @applySelfEnterDataFee = WillSelfEnterShipmentData FROM (
			SELECT WillSelfEnterShipmentData 
			From [Notification].[ShipmentInfo]
			where NotificationId = @notificationId
			UNION
			SELECT WillSelfEnterShipmentData 
			From [ImportNotification].[Shipment]
			where ImportNotificationId = @notificationId
		) AS shipmentInfo
		IF @applySelfEnterDataFee = 0
		BEGIN
			SET @price += (@numberOfShipments * (select [Value] from [Lookup].[SystemSettings] where Id = 3))
		END
	END;

	INSERT @PricingInfo
	SELECT @price, @potentialRefund;
	
	RETURN;
END;
GO