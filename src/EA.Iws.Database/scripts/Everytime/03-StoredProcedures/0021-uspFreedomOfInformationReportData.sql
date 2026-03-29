IF OBJECT_ID('[Reports].[uspFreedomOfInformationReportData]') IS NULL
	EXEC('CREATE PROCEDURE [Reports].[uspFreedomOfInformationReportData] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar Bangarugari
-- Create date: 29-03-2026
-- Description:	Get Freedom Of Information report data
-- =============================================
ALTER PROCEDURE [Reports].[uspFreedomOfInformationReportData]
	@CompetentAuthority INT,
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SELECT DISTINCT
		[NotificationNumber],
		[ImportOrExport],
		CASE WHEN [IsInterim] = 1 THEN 'Interim' WHEN [IsInterim] = 0 THEN 'Non-interim' ELSE NULL END AS [Interim],
		[BaselOecdCode],
		[NotifierName],
		[NotifierAddress],
		[NotifierPostalCode],
		[NotifierType],
		[NotifierContactName],
		[NotifierContactEmail],
		[ProducerName],
		[ProducerAddress],
		[ProducerPostalCode],
		[ProducerType],
		[ProducerContactEmail],
		[PointOfExport],
		[PointOfEntry],
		[ExportCountryName],
		[ImportCountryName],
		[TransitStates],
		[NameOfWaste],
		[WasteComponentTypes],
		[EWC],
		[YCode],
		[HCode],
		[OperationCodes],
		[ImporterName],
		[ImporterAddress],
		[ImporterPostalCode],
		[ImporterType],
		[ImporterContactName],
		[ImporterContactEmail],
		[FacilityName],
		[FacilityAddress],
		[FacilityPostalCode],
		[TechnologyEmployed],
		COALESCE(
			(SELECT	SUM(
				CASE WHEN [MovementQuantityReceviedUnitId] IN (1, 2) -- Tonnes / Cubic Metres
					THEN COALESCE([MovementQuantityReceived], 0)
				ELSE 
					COALESCE([MovementQuantityReceived] / 1000, 0) -- Convert to Tonnes / Cubic Metres
				END
				)
			), 0) AS [QuantityReceived],
		CASE WHEN [IntendedQuantityUnitId] IN (1, 2) -- Due to conversion units will only be Tonnes / Cubic Metres
			THEN [IntendedQuantityUnit] 
		WHEN [IntendedQuantityUnitId] = 3 THEN 'Tonnes'
		WHEN [IntendedQuantityUnitId] = 4 THEN 'Cubic Metres'
		END AS [QuantityReceivedUnit],
		[IntendedQuantity],
		[IntendedQuantityUnit],
		[ConsentFrom],
		[ConsentTo],
		[NotificationStatus],
		[NotificationStatusAtFileClosed],
		[DecisionRequiredByDate],
		[IsFinancialGuaranteeApproved],
		[FileClosedDate],
		[LocalArea],
		[Officer]
	FROM 
		[Reports].[FreedomOfInformationCache]
	WHERE 
		[CompetentAuthorityId] = @competentAuthority AND [ReceivedDate] BETWEEN @FromDate AND @ToDate
	GROUP BY
		[NotificationNumber],
		[ImportOrExport],
		[IsInterim],
		[BaselOecdCode],
		[NotifierName],
		[NotifierAddress],
		[NotifierPostalCode],
		[NotifierType],
		[NotifierContactName],
		[NotifierContactEmail],
		[ProducerName],
		[ProducerAddress],
		[ProducerPostalCode],
		[ProducerType],
		[ProducerContactEmail],
		[PointOfExport],
		[PointOfEntry],
		[ExportCountryName],
		[ImportCountryName],
		[TransitStates],
		[NameOfWaste],
		[WasteComponentTypes],
		[EWC],
		[YCode],
		[HCode],
		[OperationCodes],
		[ImporterName],
		[ImporterAddress],
		[ImporterPostalCode],
		[ImporterType],
		[ImporterContactName],
		[ImporterContactEmail],
		[FacilityName],
		[FacilityAddress],
		[FacilityPostalCode],
		[TechnologyEmployed],
		[IntendedQuantityUnitId],
		[IntendedQuantityUnit],
		[IntendedQuantity],
		[IntendedQuantityUnit],
		[ConsentFrom],
		[ConsentTo],
		[NotificationStatus],
		[NotificationStatusAtFileClosed],
		[DecisionRequiredByDate],
		[IsFinancialGuaranteeApproved],
		[FileClosedDate],
		[LocalArea],
		[Officer]
END
GO
