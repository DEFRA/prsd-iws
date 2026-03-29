IF OBJECT_ID('[Reports].[uspShipmentReportData]') IS NULL
	EXEC('CREATE PROCEDURE [Reports].[uspShipmentReportData] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar Bangarugari
-- Create date: 29-03-2026
-- Description:	Get Shipment report data
-- =============================================
ALTER PROCEDURE [Reports].[uspShipmentReportData]
	@CompetentAuthority INT,
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SET NOCOUNT ON;
		SELECT DISTINCT
			[NotificationNumber],
			[ImportOrExport],
			[Exporter],
			[NotifierCompanyType],
			[Importer],
			[ConsigneeCompanyType],
			[Facility],
			[FacilityCompanyType],
			[BaselOecdCode],
			[ShipmentNumber],
			[ActualDateOfShipment],
			[ConsentFrom],
			[ConsentTo],
			[PrenotificationDate],
			[ReceivedDate],
			[CompletedDate],
			[QuantityReceived],
			[QuantityReceivedUnitId] AS [Units],
			[ChemicalCompositionTypeId],
			[ChemicalComposition],
			[LocalArea],
			[TotalQuantity],
			[TotalQuantityUnitsId],
			[EntryPort],
			[DestinationCountry],
			[ExitPort],
			[OriginatingCountry],
			[Status],
			[EwcCodes],
			[OperationCodes],
			[RejectedQuantity],
			[RejectedShipmentDate],
			[RejectedReason],
			CASE WHEN YCode IS NULL THEN 'NA' ELSE YCode END AS [YCode],
			CASE WHEN HCode IS NULL THEN 'NA' ELSE HCode END AS [HCode],
			CASE WHEN UNClass IS NULL THEN 'NA' ELSE UNClass END AS [UNClass],
			[ActionedByExternalUser]
		FROM
			[Reports].[ShipmentsCache]
		WHERE
			[CompetentAuthorityId] = @CompetentAuthority AND 
			[NotificationReceivedDate] BETWEEN @FromDate AND @ToDate
		ORDER BY
			[NotificationNumber],
			[ShipmentNumber]
END
GO