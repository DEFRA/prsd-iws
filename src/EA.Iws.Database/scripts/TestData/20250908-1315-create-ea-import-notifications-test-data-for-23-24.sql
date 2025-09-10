-- Create an import notification (by an internal user) for testing the CreatedBy column in the report is correct

DECLARE @ImportNotificationId UNIQUEIDENTIFIER;
DECLARE @NotificationNumber NVARCHAR(50);
DECLARE @NotificationCreateDate DATETIME;
DECLARE @Counter INT;
DECLARE @FacilityCollectionId UNIQUEIDENTIFIER;
DECLARE @ProducerId UNIQUEIDENTIFIER;
DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @NotificationStatus INT;
DECLARE @NotificationType INT;
DECLARE @NumberOfShipments INT;

SELECT @UserId = Id
	FROM   [Identity].[AspNetUsers]
	WHERE  [Email] = 'superuser@environment-agency.gov.uk';

SET @Counter = 0;
WHILE ( @Counter < 14)
BEGIN
	SET @NotificationCreateDate = CONVERT(DATETIME2, '2023-04-01');
	SET @Counter = @Counter + 1;
	SET @NotificationStatus = 2;

IF @Counter <= 9
	BEGIN
		SET @NotificationNumber = 'GB 0001 00800' + CONVERT(VARCHAR, @Counter);
	END
ELSE
	BEGIN
		SET @NotificationNumber = 'GB 0001 0080' + CONVERT(VARCHAR, @Counter);
	END

IF @Counter <= 7
	BEGIN
		SET @NotificationType = 1;
	END
ELSE
	BEGIN
		SET @NotificationType = 2;
	END

INSERT INTO [ImportNotification].[Notification]
            ([Id]
            ,[NotificationNumber]
            ,[NotificationType]
            ,[CompetentAuthority])
     VALUES
            (NEWID(),
            @NotificationNumber,
            @NotificationType,
            1);

SET @ImportNotificationId = (SELECT Id FROM [ImportNotification].[Notification] WHERE NotificationNumber = @NotificationNumber);

INSERT INTO [ImportNotification].[InterimStatus]
           ([Id]
           ,[IsInterim]
           ,[ImportNotificationId])
     VALUES
           (NEWID(),
           1,
           @ImportNotificationId)

DECLARE @UKCountryId UNIQUEIDENTIFIER;
SELECT @UKCountryId = Id
	FROM   [Lookup].[Country]
	WHERE  [Name] = 'United Kingdom';

SET @FacilityCollectionId = NEWID();
INSERT [ImportNotification].[FacilityCollection] 
		([Id],
		 [ImportNotificationId],
		 [AllFacilitiesPreconsented])
VALUES (@FacilityCollectionId,
		@ImportNotificationId,
		1);

INSERT INTO [ImportNotification].[Facility]
           ([Id]
           ,[FacilityCollectionId]
           ,[Name]
           ,[Type]
           ,[RegistrationNumber]
           ,[Address1]
           ,[Address2]
           ,[TownOrCity]
           ,[PostalCode]
           ,[CountryId]
           ,[ContactName]
           ,[Telephone]
           ,[Email]
           ,[IsActualSiteOfTreatment])
     VALUES
           (NEWID(),
           @FacilityCollectionId,
           N'Waste Treatment Facility',
           1,
           N'45645684546',
           N'5 Bib House',
           N'The Road',
           N'Brent',
           N'HJ3 7JI',
           @UKCountryId,
           N'Bob',
           N'09876 674534',
           N'lkjlj@asfasf.faa',
           1);

INSERT INTO [ImportNotification].[Importer]
           ([Id]
           ,[ImportNotificationId]
           ,[Name]
           ,[Type]
           ,[RegistrationNumber]
           ,[Address1]
           ,[Address2]
           ,[TownOrCity]
           ,[PostalCode]
           ,[CountryId]
           ,[ContactName]
           ,[Telephone]
           ,[Email])
     VALUES
           (NEWID(),
           @ImportNotificationId,
           N'ImportersRUS',
           2,
           N'1234',
           N'Importer House',
           NULL,
           N'Hull',
           N'B78 89UI',
           @UKCountryId,
           N'Fred',
           N'44-3336669999',
           N'test@importer.com');

DECLARE @GreeceCountryId UNIQUEIDENTIFIER;
SELECT @GreeceCountryId = Id
	FROM   [Lookup].[Country]
	WHERE  [Name] = 'Greece';

INSERT INTO [ImportNotification].[Producer]
           ([Id]
           ,[ImportNotificationId]
           ,[Name]
           ,[Address1]
           ,[Address2]
           ,[TownOrCity]
           ,[PostalCode]
           ,[CountryId]
           ,[ContactName]
           ,[Telephone]
           ,[Email]
           ,[IsOnlyProducer])
     VALUES
           (NEWID(),
           @ImportNotificationId,
		   N'Producing Is Us',
           N'Producer House',
           NULL,
           N'Athens',
           N'B78 89UI',
           @GreeceCountryId,
           N'Fred',
           N'44-3336669999',
           N'test@importer.com',
           1);

INSERT INTO [ImportNotification].[Exporter]
           ([Id]
           ,[ImportNotificationId]
           ,[Name]
           ,[Address1]
           ,[Address2]
           ,[TownOrCity]
           ,[PostalCode]
           ,[CountryId]
           ,[ContactName]
           ,[Telephone]
           ,[Email])
     VALUES
           (NEWID(),
           @ImportNotificationId,
           N'Exporting Is Us',
           N'Exporter House',
           NULL,
           N'Athens',
           N'B78 89UI',
           @GreeceCountryId,
           N'Jim',
           N'44-3336667777',
           N'test@exporter.com');

IF (@Counter = 1 OR @Counter = 8)
	BEGIN
		SET @NumberOfShipments = 1;
	END
ELSE IF (@Counter = 2 OR @Counter = 9)
	BEGIN
		SET @NumberOfShipments = 6;
	END
ELSE IF (@Counter = 3 OR @Counter = 10)
	BEGIN
		SET @NumberOfShipments = 21;
	END
ELSE IF (@Counter = 4 OR @Counter = 11)
	BEGIN
		SET @NumberOfShipments = 101;
	END
ELSE IF (@Counter = 5 OR @Counter = 12)
	BEGIN
		SET @NumberOfShipments = 301;
	END
ELSE IF (@Counter = 6 OR @Counter = 13)
	BEGIN
		SET @NumberOfShipments = 501;
	END
ELSE
	BEGIN
		SET @NumberOfShipments = 1001;
	END

INSERT INTO [ImportNotification].[Shipment]
           ([Id]
           ,[NumberOfShipments]
           ,[Quantity]
           ,[Units]
           ,[FirstDate]
           ,[LastDate]
           ,[ImportNotificationId])
     VALUES
           (NEWID(),
           @NumberOfShipments,
		   Cast(25000.0000 AS DECIMAL(18, 4)),
           3,
           Cast(N'2023-04-01' AS DATE),
           Cast(N'2024-03-30' AS DATE),
           @ImportNotificationId);


DECLARE @TransportRouteId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [ImportNotification].[TransportRoute]
			([Id],
			 [ImportNotificationId])
	VALUES (@TransportRouteId, 
			@ImportNotificationId);

DECLARE @StateOfExportCountryId UNIQUEIDENTIFIER;
SELECT @StateOfExportCountryId = Id
	FROM   [Lookup].[Country]
	WHERE  [Name] = 'France';

DECLARE @CAId UNIQUEIDENTIFIER;
SELECT @CAId = Id
	FROM   [Lookup].[CompetentAuthority]
	WHERE  [Code] = 'F';

DECLARE @ExitId UNIQUEIDENTIFIER;
SELECT @ExitId = Id
	FROM   [Notification].[EntryOrExitPoint]
	WHERE  [Name] = 'Bayonne';

INSERT INTO [ImportNotification].[StateOfExport]
           ([Id]
           ,[TransportRouteId]
           ,[CountryId]
           ,[CompetentAuthorityId]
           ,[ExitPointId])
     VALUES
           (NEWID(),
           @TransportRouteId,
           @StateOfExportCountryId,
           @CAId,
           @ExitId)

SELECT @CAId = Id
	FROM   [Lookup].[CompetentAuthority]
	WHERE  [Code] = 'GB01';

DECLARE @EntryId UNIQUEIDENTIFIER;
SELECT @EntryId = Id
	FROM   [Notification].[EntryOrExitPoint]
	WHERE  [Name] = 'Dover';

INSERT INTO [ImportNotification].[StateOfImport]
           ([Id]
           ,[TransportRouteId]
           ,[CompetentAuthorityId]
           ,[EntryPointId])
     VALUES
           (NEWID(),
           @TransportRouteId,
           @CAId,
           @EntryId);

DECLARE @CountryId UNIQUEIDENTIFIER;
SELECT @CountryId = Id
	FROM   [Lookup].[Country]
	WHERE  [Name] = 'France';

SELECT @CAId = Id
	FROM   [Lookup].[CompetentAuthority]
	WHERE  [Code] = 'F';

SELECT @EntryId = Id
	FROM   [Notification].[EntryOrExitPoint]
	WHERE  [Name] = 'Calais';

SELECT @ExitId = Id
	FROM   [Notification].[EntryOrExitPoint]
	WHERE  [Name] = 'Lille';

INSERT INTO [ImportNotification].[TransitState]
           ([Id]
           ,[TransportRouteId]
           ,[CountryId]
           ,[CompetentAuthorityId]
           ,[EntryPointId]
           ,[ExitPointId]
           ,[OrdinalPosition])
     VALUES
           (NEWID(),
           @TransportRouteId,
           @CountryId,
           @CAId,
           @EntryId,
           @ExitId,
           1);

DECLARE @WasteOperationId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [ImportNotification].[WasteOperation]
           ([Id]
           ,[ImportNotificationId]
           ,[TechnologyEmployed])
     VALUES
           (@WasteOperationId,
           @ImportNotificationId,
           N'Mulching');

IF @NotificationType = 1
    BEGIN
        INSERT INTO [ImportNotification].[OperationCodes]
                   ([Id]
                   ,[WasteOperationId]
                   ,[OperationCode])
             VALUES
                   (NEWID(),
                   @WasteOperationId,
                   1);
    END
ELSE
    BEGIN
        INSERT INTO [ImportNotification].[OperationCodes]
                   ([Id]
                   ,[WasteOperationId]
                   ,[OperationCode])
             VALUES
                   (NEWID(),
                   @WasteOperationId,
                   14);
    END

DECLARE @WasteTypeId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [ImportNotification].[WasteType]
           ([Id]
           ,[ImportNotificationId]
           ,[Name]
           ,[BaselOecdCodeNotListed]
           ,[YCodeNotApplicable]
           ,[HCodeNotApplicable]
           ,[UnClassNotApplicable]
		   ,[ChemicalCompositionType])
     VALUES
           (@WasteTypeId,
           @ImportNotificationId,
           N'Rubish',
           1,
           1,
           1,
           1,
		   1);

INSERT INTO [ImportNotification].[WasteCode]
           ([Id]
           ,[WasteTypeId]
           ,[WasteCodeId])
     VALUES
           (NEWID(),
           @WasteTypeId,
           (SELECT TOP 1 Id FROM [Lookup].[WasteCode] WHERE [Code] = '01 01 01' AND [CodeType] = 3 ));

DECLARE @NotificationAssessmentId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [ImportNotification].[NotificationAssessment]
           ([Id]
           ,[NotificationApplicationId]
           ,[Status])
     VALUES
           (@NotificationAssessmentId,
		   @ImportNotificationId,
           @NotificationStatus);

INSERT INTO [ImportNotification].[NotificationStatusChange]
           ([Id]
           ,[NotificationAssessmentId]
           ,[PreviousStatus]
           ,[NewStatus]
           ,[UserId]
           ,[ChangeDate])
     VALUES
           (NEWID()
           ,@NotificationAssessmentId
           ,1
           ,2
           ,@UserId
           ,@NotificationCreateDate)

INSERT INTO [ImportNotification].[NotificationDates]
           ([Id]
           ,[NotificationAssessmentId]
           ,[NotificationReceivedDate]
           ,[PaymentReceivedDate]
           ,[AssessmentStartedDate]
           ,[NameOfOfficer]
           ,[NotificationCompletedDate]
           ,[AcknowledgedDate]
           ,[ConsentedDate])
     VALUES
           (NEWID(),
           @NotificationAssessmentId,
           Cast(N'2023-04-01' AS DATE),
           Cast(N'2023-04-02' AS DATE),
           Cast(N'2023-04-03' AS DATE),
           N'Santa',
           Cast(N'2023-04-04' AS DATE),
           Cast(N'2023-04-05' AS DATE),
           Cast(N'2023-04-06' AS DATE));

INSERT INTO [ImportNotification].[Consent]
           ([Id]
           ,[From]
           ,[To]
           ,[Conditions]
           ,[UserId]
           ,[NotificationId])
     VALUES
           (NEWID(),
           Cast(N'2023-04-01' AS DATE),
           Cast(N'2023-03-30' AS DATE),
           N'Be nice',
           @UserId,
           @ImportNotificationId);
END
