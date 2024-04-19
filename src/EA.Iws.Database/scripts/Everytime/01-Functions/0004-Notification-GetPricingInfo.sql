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
	DECLARE @additionalChargeTotal MONEY;
	
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

	--Getting Notification Additional charge totals
	SET @additionalChargeTotal = ISNULL((SELECT SUM(ChargeAmount) FROM [Notification].[AdditionalCharges] WHERE NotificationId = @notificationId), 0);
	SET @additionalChargeTotal += ISNULL((SELECT SUM(ChargeAmount) FROM [ImportNotification].[AdditionalCharges] WHERE NotificationId = @notificationId), 0);

	IF @competentAuthority = 1 AND @submittedDate >= (SELECT [Value] from [Lookup].[SystemSettings] where Id = 1) 
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
			SELECT @fixedWasteCategoryFee += ISNULL(@additionalChargeTotal, 0);
			INSERT @PricingInfo
			SELECT @fixedWasteCategoryFee, 0;
			RETURN;
		END;

		IF @numberOfShipments > 1000
		BEGIN
			DECLARE @hundreds INT
			SET @hundreds = (@numberOfShipments - 901) / 100

			--Import Recovery IsInterim 0, ActivityId = F0407E39-C9BA-4519-B659-A4C9010901C7
			--Import Recovery IsInterim 1, ActivityId = DAF51836-41E0-4324-93AE-A4C9010901C7
			--Import Disposal IsInterim 0, ActivityId = E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7
			--Import Disposal IsInterim 1, ActivityId = BE00F07B-41E1-4C03-9BB9-A4C9010901C7
			IF @activityId IN (
				'F0407E39-C9BA-4519-B659-A4C9010901C7', 'DAF51836-41E0-4324-93AE-A4C9010901C7', 
				'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7')
			BEGIN
				SET @price += (SELECT [VALUE] * @hundreds FROM [Lookup].[SystemSettings] where Id = 4)
			END
			
			--Export Recovery IsInterim 0, ActivityId = 75496653-C767-44D2-AA27-A4C9010901C7
			--Export Recovery IsInterim 1, ActivityId = 71CC7688-63D3-4312-BAD2-A4C9010901C7
			IF @activityId IN ('75496653-C767-44D2-AA27-A4C9010901C7', '71CC7688-63D3-4312-BAD2-A4C9010901C7')
			BEGIN
				SET @price += (SELECT [VALUE] * @hundreds FROM [Lookup].[SystemSettings] where Id = 7)
			END

			--Export Disposal IsInterim 0, ActivityId = 12AF7EA4-1E60-4D35-B965-A4C9010901C7
			--Export Disposal IsInterim 1, ActivityId = 8385CAD7-E5F0-4765-A46B-A4C9010901C7
			IF @activityId IN ('12AF7EA4-1E60-4D35-B965-A4C9010901C7', '8385CAD7-E5F0-4765-A46B-A4C9010901C7')
			BEGIN
				SET @price += (SELECT [VALUE] * @hundreds FROM [Lookup].[SystemSettings] where Id = 8)
			END

			--ELSE
			--BEGIN
			--	SET @price += (@price * 0.10 * @hundreds)
			--END

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

	IF @competentAuthority = 2 AND @submittedDate >= (SELECT [Value] from [Lookup].[SystemSettings] where Id = 2) 
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
			SELECT @fixedWasteCategoryFee += ISNULL(@additionalChargeTotal, 0);
			INSERT @PricingInfo
			SELECT @fixedWasteCategoryFee, 0;
			RETURN;
		END;

		DECLARE @selfEnteringData bit;
		SELECT @selfEnteringData = WillSelfEnterShipmentData FROM (
			SELECT WillSelfEnterShipmentData 
			From [Notification].[ShipmentInfo]
			where NotificationId = @notificationId
		) AS shipmentInfo
		IF @selfEnteringData = 0
		BEGIN
			SET @price += (@numberOfShipments * (select [Value] from [Lookup].[SystemSettings] where Id = 3))
		END
	END;	

	SELECT @price += ISNULL(@additionalChargeTotal, 0);
	INSERT @PricingInfo
	SELECT @price, @potentialRefund;
	
	RETURN;
END;
