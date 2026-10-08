IF OBJECT_ID('[Search].[uspGetNotifications]') IS NULL
	EXEC('CREATE PROCEDURE [Search].[uspGetNotifications] AS SET NOCOUNT ON;')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- ======================================================================================
-- Author:		Sreedhar B
-- Create date: 22/06/2026
-- Description:	To get the list of notifications to display advanced search result
-- ======================================================================================
ALTER PROCEDURE [Search].[uspGetNotifications]
(
	@ca INT,
	@importOrExport NVARCHAR(50),
	@ewc NVARCHAR(50) = NULL,
	@baselOecd NVARCHAR(50) = NULL,
	@producerName NVARCHAR(255) = NULL,
	@importerName NVARCHAR(255) = NULL,
	@exporterName NVARCHAR(255) = NULL,
	@facilityName NVARCHAR(255) = NULL,
	@importCountryName NVARCHAR(255) = NULL,
	@exitPointName NVARCHAR(255) = NULL,
	@entryPointName NVARCHAR(255) = NULL,
	@localAreaId UNIQUEIDENTIFIER = NULL,
	@consentValidFromStart DATETIME = NULL,
	@consentValidFromEnd DATETIME = NULL,
	@consentValidToStart DATETIME = NULL,
	@consentValidToEnd DATETIME = NULL,
	@notificationReceivedStart DATETIME = NULL,
	@notificationReceivedEnd DATETIME = NULL,
	@notificationType NVARCHAR(50) = NULL,
	@exportStatus NVARCHAR(50) = NULL,
	@importStatus NVARCHAR(50) = NULL,
	@isInterim BIT = NULL,
	@exportCountryName NVARCHAR(255) = NULL,
	@operationCodes NVARCHAR(50) = NULL,
	@baselOecdCodeNotListed BIT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT [Id]
	FROM [Search].[Notifications]
	WHERE [CompetentAuthority] = @ca
		AND [ImportOrExport] = @importOrExport

		AND (@ewc IS NULL OR [EwcCode] LIKE '%' + @ewc + '%')
		AND (@baselOecd IS NULL OR [BaselOecdCode] LIKE '%' + @baselOecd + '%')

	AND (
			@producerName IS NULL
			OR [ProducerName] LIKE '%' + @producerName + '%'
			OR [ProducerRegNumber] LIKE '%' + @producerName + '%'
		)

	AND (
			@importerName IS NULL
			OR [ImporterName] LIKE '%' + @importerName + '%'
			OR [ImporterRegNumber] LIKE '%' + @importerName + '%'
		)

	AND (
			@exporterName IS NULL
			OR [ExporterName] LIKE '%' + @exporterName + '%'
			OR [ExporterRegNumber] LIKE '%' + @exporterName + '%'
		)

	AND (
			@facilityName IS NULL
			OR [FacilityName] LIKE '%' + @facilityName + '%'
			OR [FacilityRegNumber] LIKE '%' + @facilityName + '%'
		)

	AND (@importCountryName IS NULL OR [CountryOfImport] LIKE '%' + @importCountryName + '%')
	AND (@exitPointName IS NULL OR [ExitPointName] LIKE '%' + @exitPointName + '%')
	AND (@entryPointName IS NULL OR [EntryPointName] LIKE '%' + @entryPointName + '%')

	AND (@localAreaId IS NULL OR [LocalAreaId] = @localAreaId)

	AND (
			@consentValidFromStart IS NULL
			OR [ConsentValidFrom] BETWEEN @consentValidFromStart AND COALESCE(@consentValidFromEnd, GETDATE())
		)

	AND (
			@consentValidToStart IS NULL
			OR [ConsentValidTo] BETWEEN @consentValidToStart AND COALESCE(@consentValidToEnd, GETDATE())
		)

	AND (
			@notificationReceivedStart IS NULL
			OR [NotificationReceivedDate] BETWEEN @notificationReceivedStart AND COALESCE(@notificationReceivedEnd, GETDATE())
		)

	AND (@notificationType IS NULL OR [NotificationType] = @notificationType)
	AND (@exportStatus IS NULL OR [ExportStatus] = @exportStatus)
	AND (@importStatus IS NULL OR [ImportStatus] = @importStatus)
	AND (@isInterim IS NULL OR [IsInterim] = @isInterim)

	AND (@exportCountryName IS NULL OR [CountryOfExport] LIKE '%' + @exportCountryName + '%')
	AND (@operationCodes IS NULL OR [OperationCodes] = @operationCodes)
	AND (@baselOecdCodeNotListed IS NULL OR [BaselOecdCodeNotListed] = @baselOecdCodeNotListed);
END;
GO