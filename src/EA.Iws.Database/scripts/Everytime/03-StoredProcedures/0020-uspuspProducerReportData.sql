IF OBJECT_ID('[Reports].[uspProducerReportData]') IS NULL
	EXEC('CREATE PROCEDURE [Reports].[uspProducerReportData] AS SET NOCOUNT ON;')
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
ALTER PROCEDURE [Reports].[uspProducerReportData]
	@CompetentAuthority INT,
	@FromDate DATE,
	@ToDate DATE
AS
BEGIN
	SELECT DISTINCT
		[NotificationNumber],
		[NotifierName],
		[ProducerName],
		[ProducerAddress1],
		[ProducerAddress2],
		[ProducerTownOrCity],
		[ProducerPostCode],
		[SiteOfExport],
		[LocalArea],
		[WasteType],
		[NotificationStatus],
		[ConsigneeName]
	FROM
		[Reports].[ProducerCache]
	WHERE
		[CompetentAuthorityId] = @competentAuthority AND [NotificationReceivedDate] BETWEEN @FromDate AND @ToDate
	ORDER BY
		[NotificationNumber]
END
GO
