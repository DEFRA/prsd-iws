DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @ImportNotificationId UNIQUEIDENTIFIER = 'ceb03ae8-1234-4dfc-8f87-6b2384f00001';

SELECT @UserId = id
FROM   [Identity].[aspnetusers]
WHERE  [email] = 'superuser@environment-agency.gov.uk'

INSERT INTO [ImportNotification].[Notification]
           ([Id]
           ,[NotificationNumber]
           ,[NotificationType]
           ,[CompetentAuthority])
     VALUES
           (@ImportNotificationId,
           N'GB 0001 007701',
           2,
           1) 

INSERT INTO [ImportNotification].[InterimStatus]
           ([Id]
           ,[IsInterim]
           ,[ImportNotificationId])
     VALUES
           (NEWID(),
           1,
           @ImportNotificationId)

DECLARE @FacilityCollectionId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [ImportNotification].[FacilityCollection]
           ([Id]
           ,[ImportNotificationId]
           ,[AllFacilitiesPreconsented])
     VALUES
           (@FacilityCollectionId,
           @ImportNotificationId,
           1)

DECLARE @UKCountryId UNIQUEIDENTIFIER;
SELECT @UKCountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'United Kingdom';

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
           1)

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
           N'Scrap Waste Reclamation',
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
           0)

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
           N'test@importer.com')

DECLARE @GreeceCountryId UNIQUEIDENTIFIER;
SELECT @GreeceCountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'Greece';

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
           1)

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
           N'test@exporter.com')

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
           520,
		   Cast(25000.0000 AS DECIMAL(18, 4)),
           3,
           Cast(N'2015-09-01' AS DATE),
           Cast(N'2016-08-27' AS DATE),
           @ImportNotificationId)


DECLARE @TransportRouteId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [ImportNotification].[TransportRoute]
([Id], [ImportNotificationId])
VALUES (@TransportRouteId, 
		@ImportNotificationId)

DECLARE @GermanyCountryId UNIQUEIDENTIFIER;
SELECT @GermanyCountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'Germany';

DECLARE @CAId UNIQUEIDENTIFIER;
SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'DE018';

DECLARE @ExitId UNIQUEIDENTIFIER;
SELECT @ExitId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Aachen';

INSERT INTO [ImportNotification].[StateOfExport]
           ([Id]
           ,[TransportRouteId]
           ,[CountryId]
           ,[CompetentAuthorityId]
           ,[ExitPointId])
     VALUES
           (NEWID(),
           @TransportRouteId,
           @GermanyCountryId,
           @CAId,
           @ExitId)

SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'GB01';

DECLARE @EntryId UNIQUEIDENTIFIER;
SELECT @EntryId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Dover';

INSERT INTO [ImportNotification].[StateOfImport]
           ([Id]
           ,[TransportRouteId]
           ,[CompetentAuthorityId]
           ,[EntryPointId])
     VALUES
           (NEWID(),
           @TransportRouteId,
           @CAId,
           @EntryId)

DECLARE @CountryId UNIQUEIDENTIFIER;
SELECT @CountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'France';

SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'FR';

SELECT @EntryId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Calais';

SELECT @ExitId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Lille';

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
           1)

DECLARE @WasteOperationId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [ImportNotification].[WasteOperation]
           ([Id]
           ,[ImportNotificationId]
           ,[TechnologyEmployed])
     VALUES
           (@WasteOperationId,
           @ImportNotificationId,
           N'Mulching')

INSERT INTO [ImportNotification].[OperationCodes]
           ([Id]
           ,[WasteOperationId]
           ,[OperationCode])
     VALUES
           (NEWID(),
           @WasteOperationId,
           10)

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
		   1)

INSERT INTO [ImportNotification].[WasteCode]
           ([Id]
           ,[WasteTypeId]
           ,[WasteCodeId])
     VALUES
           (NEWID(),
           @WasteTypeId,
           (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = '01 01 01' AND [CodeType] = 3 ))


DECLARE @NotificationAssessmentId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [ImportNotification].[NotificationAssessment]
           ([Id]
           ,[NotificationApplicationId]
           ,[Status])
     VALUES
           (@NotificationAssessmentId,
		   @ImportNotificationId,
           9)


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
           Cast(N'2016-01-01' AS DATE),
           Cast(N'2016-01-02' AS DATE),
           Cast(N'2016-01-03' AS DATE),
           N'Santa',
           Cast(N'2016-01-04' AS DATE),
           Cast(N'2016-01-05' AS DATE),
           Cast(N'2016-01-06' AS DATE))


INSERT INTO [ImportNotification].[Consent]
           ([Id]
           ,[From]
           ,[To]
           ,[Conditions]
           ,[UserId]
           ,[NotificationId])
     VALUES
           (NEWID(),
           Cast(N'2016-01-01' AS DATE),
           Cast(N'2016-12-31' AS DATE),
           N'Be nice',
           @UserId,
           @ImportNotificationId)


