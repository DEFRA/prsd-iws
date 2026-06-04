IF OBJECT_ID('[Reports].[uspFinanceReportData]') IS NULL
	EXEC('CREATE PROCEDURE [Reports].[uspFinanceReportData] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Sreedhar Bangarugari
-- Create date: 29-03-2026
-- Description:	Get Finance report data
-- =============================================
ALTER PROCEDURE [Reports].[uspFinanceReportData]
	@CompetentAuthority INT,
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SELECT
		[NotificationNumber],
		[CreatedBy],
		[Notifier],
		[NotifierAddress],
		[NotifierPostalCode],
		[Consignee],
		[ConsigneeAddress],
		[ConsigneePostalCode],
		[Facility],
		[FacilityAddress],
		[FacilityPostalCode],
		[ReceivedDate],
		[PaymentReceivedDate],
		[TotalBillable],
		[TotalPaid],
		[LatestPaymentDate],
		[AmountToRefund],
		[TotalRefunded],
		[LatestRefundDate],
		[IntendedNumberOfShipments],
		[IntendedQuantity],
		[Units],
		[TotalShipmentsMade],
		[ImportOrExport],
		[NotificationType],
		[Preconsented],
		[HasMultipleFacilities],
		[ConsentFrom],
		[ConsentTo],
		[Status],
		[IsInterim],
		[PaymentComments]
	FROM
		[Reports].[Finance]
	WHERE
		[CompetentAuthorityId] = @CompetentAuthority AND
		[ReceivedDate] BETWEEN @FromDate AND @ToDate
	ORDER BY
		[NotificationNumber]
END
GO
