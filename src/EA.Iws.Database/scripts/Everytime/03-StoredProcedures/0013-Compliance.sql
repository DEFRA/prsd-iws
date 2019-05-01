IF OBJECT_ID('[Reports].[Compliance]') IS NULL
    EXEC('CREATE PROCEDURE [Reports].[Compliance] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Reports].[Compliance]
  @DateType NVARCHAR(30)
 ,@CompetentAuthority INT
 ,@From DATE
 ,@To DATE
AS
BEGIN

	SET NOCOUNT ON;

	--Final Output table
	DECLARE @ComplianceReport TABLE
	(
		NotificationId					UNIQUEIDENTIFIER NOT NULL,       
		NotificationNumber				NVARCHAR(50)  NOT NULL,     
		CompetentAuthorityId			INT NOT NULL,
		NoPrenotificationCount			INT,
   		PreNotificationColour           NVARCHAR(10),   
		MissingShipments				INT,
		MissingShipmentsColour          NVARCHAR(10),   
		OverLimitShipments				NVARCHAR(30) ,       
		OverActiveLoads					CHAR(3),  
		OverTonnage						CHAR(1),       
		OverTonnageColour               NVARCHAR(10),       
		OverShipments					CHAR(1),  
		OverShipmentsColour             NVARCHAR(10),   
		Notifier	                    NVARCHAR(255),       
		Consignee	                    NVARCHAR(255),      
		FileExpired	                    CHAR(1),
		EWCCode							NVARCHAR(MAX),
		YCode							NVARCHAR(MAX),
		PointOfExit						NVARCHAR(2048),
        PointOfEntry					NVARCHAR(2048),
		ExportCountryName				NVARCHAR(2048),
        ImportCountryName				NVARCHAR(2048),
		SiteOfExportName				NVARCHAR(2048),
		FacilityName					NVARCHAR(MAX)  
	)

	INSERT INTO @ComplianceReport EXEC [Reports].[ExportCompliance] @DateType, @CompetentAuthority, @From, @To

	INSERT INTO @ComplianceReport EXEC [Reports].[ImportCompliance]	@DateType, @CompetentAuthority, @From, @To

	SELECT * FROM @ComplianceReport
END	 