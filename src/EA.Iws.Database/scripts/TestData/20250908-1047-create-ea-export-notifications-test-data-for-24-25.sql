-- Create an export notification with an external user for testing the CreatedBy column in the report is correct

DECLARE @NotificationId UNIQUEIDENTIFIER;
DECLARE @NotificationNumber NVARCHAR(50);
DECLARE @NotificationCreateDate DATETIME;
DECLARE @Counter INT;
DECLARE @MeansOfTransportId UNIQUEIDENTIFIER;
DECLARE @FacilityCollectionId UNIQUEIDENTIFIER;
DECLARE @ProducerCollectionId UNIQUEIDENTIFIER;
DECLARE @ProducerId UNIQUEIDENTIFIER;
DECLARE @CarrierCollectionId UNIQUEIDENTIFIER;
DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @NotificationStatus INT;
DECLARE @NotificationType INT;
DECLARE @NumberOfShipments INT;

SELECT @UserId = Id
	FROM	[Identity].[aspnetusers]
	WHERE	[UserName] = 'sunily@sfwltd.co.uk';

SET @Counter = 0;
WHILE ( @Counter < 14)
BEGIN
	SET @NotificationCreateDate = CONVERT(DATETIME2, '2024-04-01');
	SET @Counter = @Counter + 1;
	SET @NotificationStatus = 2;

IF @Counter <= 9
	BEGIN
		SET @NotificationNumber = 'GB 0001 00900' + CONVERT(VARCHAR, @Counter);
	END
ELSE
	BEGIN
		SET @NotificationNumber = 'GB 0001 0090' + CONVERT(VARCHAR, @Counter);
	END

IF @Counter <= 7
	BEGIN
		SET @NotificationType = 1;
	END
ELSE
	BEGIN
		SET @NotificationType = 2;
	END

INSERT [Notification].[Notification]
	   ([Id],
	    [UserId],
		[NotificationType],
		[CompetentAuthority],
		[NotificationNumber],
		[CreatedDate],
		[ReasonForExport],
		[HasSpecialHandlingRequirements],
		[SpecialHandlingDetails],
		[IsRecoveryPercentageDataProvidedByImporter],
		[WasteGenerationProcess],
		[IsWasteGenerationProcessAttached])
VALUES (NEWID(),
		@UserId,
		@NotificationType,
		1,
		@NotificationNumber,
		@NotificationCreateDate,
		NULL,
		0,
		NULL,
		1,
		NULL,
		1)

SET @NotificationId = (SELECT Id FROM [Notification].[Notification] WHERE NotificationNumber = @NotificationNumber)

SET @MeansOfTransportId = NEWID();
INSERT [Notification].[MeansOfTransport]
	   ([Id],
		[NotificationId],
		[MeansOfTransport])
VALUES (@MeansOfTransportId,
		@NotificationId,
		N'R;S;R;A;R');

SET @FacilityCollectionId = NEWID();
INSERT [Notification].[FacilityCollection] 
		([Id],
		 [NotificationId],
		 [AllFacilitiesPreconsented])
VALUES (@FacilityCollectionId,
		@NotificationId,
		1)

INSERT [Notification].[Facility]
	   ([Id],
		[Name],
		[IsActualSiteOfTreatment],
		[Type],
		[RegistrationNumber],
		[AdditionalRegistrationNumber],
		[Address1],
		[Address2],
		[TownOrCity],
		[PostalCode],
		[Region],
		[Country],
		[FullName],
		[Telephone],
		[Fax],
		[Email],
		[FacilityCollectionId],
		[OtherDescription])
VALUES (NEWID(),
		N'Importer Facility',
		1,
		1,
		N'Micky Finns Imports',
		NULL,
		N'Opp. ABCD',
		N'Near XYZ',
		N'Dortmund',
		N'64151',
		N'Dortmund',
		N'Germany',
		N'Maxwell Bob',
		N'53-2225557777',
		N'53-3336669999',
		N'test@importer.de',
		@FacilityCollectionId,
		NULL)

INSERT [Notification].[Importer]
	   ([Id],
		[Name],
		[Type],
		[RegistrationNumber],
		[AdditionalRegistrationNumber],
		[Address1],
		[Address2],
		[TownOrCity],
		[PostalCode],
		[Region],
		[Country],
		[FullName],
		[Telephone],
		[Fax],
		[Email],
		[NotificationId],
		[OtherDescription])
VALUES (NEWID(),
		N'Importer',
		2,
		N'IMPORTER123',
		NULL,
		N'Opp. ABCD',
		N'Near XYZ',
		N'Woking',
		N'GU22 7UY',
		N'Surrey',
		N'United Kingdom',
		N'John Bob',
		N'44-2225557777',
		N'44-3336669999',
		N'test@importer.com',
		@NotificationId,
		NULL)

SET @ProducerCollectionId = NEWID();
INSERT [Notification].[ProducerCollection]
	   ([Id],
		[NotificationId])
VALUES (@ProducerCollectionId,
		@NotificationId)

SET @ProducerId = NEWID();
INSERT [Notification].[Producer]
	   ([Id],
		[Name],
		[IsSiteOfExport],
		[Type],
		[RegistrationNumber],
		[AdditionalRegistrationNumber],
		[Address1],
		[Address2],
		[TownOrCity],
		[PostalCode],
		[Region],
		[Country],
		[FullName],
		[Telephone],
		[Fax],
		[Email],
		[ProducerCollectionId],
		[OtherDescription])
VALUES (@ProducerId,
		N'New Producer',
		0,
		1,
		N'Not applicable',
		NULL,
		N'Station Approach',
		N'Opp. TESCO',
		N'Woking',
		N'GU22 7UY',
		N'Surrey',
		N'United Kingdom',
		N'John Smith',
		N'44-7778889999',
		N'44-1112223333',
		N'test@producer.com',
		@ProducerCollectionId,
		NULL)

INSERT [Notification].[Exporter]
	   ([Id],
		[Name],
		[Type],
		[RegistrationNumber],
		[AdditionalRegistrationNumber],
		[Address1],
		[Address2],
		[TownOrCity],
		[PostalCode],
		[Region],
		[Country],
		[FullName],
		[Telephone],
		[Fax],
		[Email],
		[NotificationId],
		[OtherDescription])
VALUES (NEWID(),
		N'Exporter',
		2,
		N'EXP12345',
		N'EXP12356',
		N'Station Approach',
		N'Opp. TESCO',
		N'Woking',
		N'GU22 7UY',
		N'Surrey',
		N'United Kingdom',
		N'John Smith',
		N'44-7778889999',
		N'44-1112223333',
		N'test@exporter.com',
		@NotificationId,
		NULL)

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

INSERT [Notification].[ShipmentInfo]
	   ([Id],
		[NotificationId],
		[NumberOfShipments],
		[Quantity],
		[Units],
		[FirstDate],
		[LastDate],
		[WillSelfEnterShipmentData])
VALUES (NEWID(),
		@NotificationId,
		@NumberOfShipments,
		Cast(1000.0000 AS DECIMAL(18, 4)),
		3,
		Cast(N'2024-04-01' AS DATE),
		Cast(N'2025-03-30' AS DATE),
		1)

SET @CarrierCollectionId = NEWID();
INSERT [Notification].[CarrierCollection]
	   ([Id],
		[NotificationId])
VALUES (@CarrierCollectionId,
		@NotificationId)

INSERT [Notification].[Carrier]
	   ([Id],
		[Name],
		[CarrierCollectionId],
		[Type],
		[RegistrationNumber],
		[AdditionalRegistrationNumber],
		[Address1],
		[Address2],
		[TownOrCity],
		[PostalCode],
		[Region],
		[Country],
		[FullName],
		[Telephone],
		[Fax],
		[Email],
		[OtherDescription])
VALUES (NEWID(),
		N'Carrier',
		@CarrierCollectionId,
		1,
		N'CARRIER12345',
		NULL,
		N'Business House',
		N'Reading Street',
		N'Guildford',
		N'GU21 5EM',
		N'Surrey',
		N'United Kingdom',
		N'Karen Murrey',
		N'44-4445556666',
		N'44-1112223333',
		N'test@carrier.com',
		NULL)

INSERT [Notification].[PackagingInfo]
	   ([Id],
		[PackagingType],
		[OtherDescription],
		[NotificationId])
VALUES (NEWID(),
		4,
		NULL,
		@NotificationId)

INSERT [Notification].[PackagingInfo]
	   ([Id],
		[PackagingType],
		[OtherDescription],
		[NotificationId])
VALUES (NEWID(),
		5,
		NULL,
		@NotificationId)

DECLARE @TransportRouteId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [Notification].[TransportRoute]
			([Id], 
			[NotificationId])
	VALUES (@TransportRouteId, 
			@NotificationId)

DECLARE @CountryId UNIQUEIDENTIFIER;

SELECT @CountryId = id
FROM   [Lookup].[Country]
WHERE  [name] = 'United Kingdom';

DECLARE @CAId UNIQUEIDENTIFIER;

SELECT @CAId = id
FROM   [Lookup].[CompetentAuthority]
WHERE  [code] = 'GB01';

DECLARE @EntryId UNIQUEIDENTIFIER;
DECLARE @ExitId UNIQUEIDENTIFIER;

SELECT @EntryId = id
FROM   [Notification].[EntryOrExitPoint]
WHERE  [name] = 'Dover';

INSERT [Notification].[StateOfExport]
	   ([id],
		[TransportRouteId],
		[countryid],
		[competentauthorityid],
		[exitpointid])
VALUES (NEWID(),
		@TransportRouteId,
		@CountryId,
		@CAId,
		@EntryId)

SELECT @CountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'France';

SELECT @CAId = id
FROM   [Lookup].[CompetentAuthority]
WHERE  [code] = 'F';

SELECT @ExitId = id
FROM   [Notification].[EntryOrExitPoint]
WHERE  [name] = 'Bayonne';

INSERT [Notification].[StateOfImport]
	   ([id],
		[TransportRouteId],
		[countryid],
		[competentauthorityid],
		[entrypointid])
VALUES (NEWID(),
		@TransportRouteId,
		@CountryId,
		@CAId,
		@ExitId)

SELECT @CountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'France';

SELECT @CAId = id
FROM   [Lookup].[CompetentAuthority]
WHERE  [code] = 'F';

SELECT @EntryId = id
FROM   [Notification].[EntryOrExitPoint]
WHERE  [name] = 'Calais';

SELECT @ExitId = id
FROM   [Notification].[EntryOrExitPoint]
WHERE  [name] = 'Lille';

INSERT [Notification].[TransitState]
	   ([id],
		[TransportRouteId],
		[countryid],
		[competentauthorityid],
		[entrypointid],
		[exitpointid],
		[ordinalposition])
VALUES (NEWID(),
		@TransportRouteId,
		@CountryId,
		@CAId,
		@EntryId,
		@ExitId,
		1)

INSERT [Notification].[OperationCodes]
	   ([id],
		[notificationid],
		[operationcode])
VALUES (NEWID(),
		@NotificationId,
		2)

INSERT [Notification].[OperationCodes]
	   ([id],
		[notificationid],
		[operationcode])
VALUES (NEWID(),
		@NotificationId,
		3)

INSERT [Notification].[OperationCodes]
	   ([id],
		[notificationid],
		[operationcode])
VALUES (NEWID(),
		@NotificationId,
		1)

DECLARE @WasteTypeId UNIQUEIDENTIFIER = NEWID();
		
INSERT [Notification].[WasteType]
	   ([id],
		[chemicalcompositiontype],
		[chemicalcompositionname],
		[chemicalcompositiondescription],
		[notificationid],
		[hasannex],
		[otherwastetypedescription],
		[energyinformation],
		[woodtypedescription],
		[optionalinformation])
VALUES (@WasteTypeId,
		3,
		NULL,
		N'Wooden blocks',
		@NotificationId,
		0,
		NULL,
		NULL,
		N'Wooden blocks',
		NULL)

INSERT [Notification].[WasteComposition]
	   ([id],
		[constituent],
		[minconcentration],
		[maxconcentration],
		[wastetypeid],
		[chemicalcompositiontype])
VALUES (NEWID(),
		N'Paper',
		Cast(1.00 AS DECIMAL(5, 2)),
		Cast(3.00 AS DECIMAL(5, 2)),
		@WasteTypeId,
		1)

INSERT [Notification].[WasteComposition]
	   ([id],
		[constituent],
		[minconcentration],
		[maxconcentration],
		[wastetypeid],
		[chemicalcompositiontype])
VALUES (NEWID(),
		N'Plastics',
		Cast(1.00 AS DECIMAL(5, 2)),
		Cast(3.00 AS DECIMAL(5, 2)),
		@WasteTypeId,
		2)

INSERT [Notification].[WasteComposition]
	   ([id],
		[constituent],
		[minconcentration],
		[maxconcentration],
		[wastetypeid],
		[chemicalcompositiontype])
VALUES (NEWID(),
		N'Wood',
		Cast(1.00 AS DECIMAL(5, 2)),
		Cast(3.00 AS DECIMAL(5, 2)),
		@WasteTypeId,
		4)

INSERT [Notification].[WasteComposition]
	   ([id],
		[constituent],
		[minconcentration],
		[maxconcentration],
		[wastetypeid],
		[chemicalcompositiontype])
VALUES (NEWID(),
		N'Textiles',
		Cast(1.00 AS DECIMAL(5, 2)),
		Cast(3.00 AS DECIMAL(5, 2)),
		@WasteTypeId,
		5)

INSERT [Notification].[WasteComposition]
	   ([id],
		[constituent],
		[minconcentration],
		[maxconcentration],
		[wastetypeid],
		[chemicalcompositiontype])
VALUES (NEWID(),
		N'Metals',
		Cast(1.00 AS DECIMAL(5, 2)),
		Cast(3.00 AS DECIMAL(5, 2)),
		@WasteTypeId,
		6)

INSERT [Notification].[PhysicalCharacteristicsInfo]
	   ([id],
		[physicalcharacteristictype],
		[otherdescription],
		[notificationid])
VALUES (NEWID(),
		4,
		NULL,
		@NotificationId)

INSERT [Notification].[PhysicalCharacteristicsInfo]
	   ([Id],
		[PhysicalCharacteristicType],
		[OtherDescription],
		[NotificationId])
VALUES (NEWID(),
		3,
		NULL,
		@NotificationId)

INSERT [Notification].[TechnologyEmployed]
	   ([Id],
		[AnnexProvided],
		[Details],
		[NotificationId],
		[FurtherDetails])
VALUES (NEWID(),
		0,
		'Electrolysis',
		@NotificationId,
		'A cathode and anode are used to separate recoverable materials from an ionic solution of waste.')

INSERT [Notification].[WasteCodeInfo]
	   ([id],
		[wastecodeid],
		[customcode],
		[notificationid],
		[codetype])
VALUES (NEWID(),
		(SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'A1030'),
		NULL,
		@NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'A1030')),
		(NEWID(),
		(SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = '01 05 04'),
		NULL,
		@NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = '01 05 04')),
		(NEWID(),
		NULL,
		N'1561231',
		@NotificationId,
		8),
		(NEWID(),
		NULL,
		N'Iron filings',
		@NotificationId,
		7),
		(NEWID(),
		NULL,
		N'GB01',
		@NotificationId,
		9),
		(NEWID(),
		NULL,
		N'XV72663',
		@NotificationId,
		10),
		(NEWID(),
		(SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [Description] = 'Explosives'),
		NULL,
		@NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [Description] = 'Explosives')),
		(NEWID(),
		(SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'Y1'),
		NULL,
		@NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'Y1')),
		(NEWID(),
		(SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'H1'),
		NULL,
		@NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'H1')),
		(NEWID(),
		(SELECT TOP 1 Id FROM [Lookup].[WasteCode] WHERE [code] = 'UN 0004'),
		NULL,
		@NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'UN 0004'));

INSERT [Notification].[WasteAdditionalInformation]
	   ([id],
		[constituent],
		[minconcentration],
		[maxconcentration],
		[wastetypeid],
		[wasteinformationtype])
VALUES (NEWID(),
		N'Chlorine',
		Cast(2.00 AS DECIMAL(5, 2)),
		Cast(5.00 AS DECIMAL(5, 2)),
		@WasteTypeId,
		5)

DECLARE @NotificationAssessmentId UNIQUEIDENTIFIER = NEWID();

INSERT [Notification].[NotificationAssessment]
	   ([Id],
	    [NotificationApplicationId],
	    [Status])
VALUES (@NotificationAssessmentId,
		@NotificationId,
		@NotificationStatus);

INSERT INTO [Notification].[NotificationStatusChange]
           ([Id]
           ,[NotificationAssessmentId]
           ,[Status]
           ,[UserId]
           ,[ChangeDate])
     VALUES
           (NEWID()
           ,@NotificationAssessmentId
           ,@NotificationStatus
           ,@UserId
           ,@NotificationCreateDate)

INSERT [Notification].[NotificationDates]
		([Id],
			[NotificationAssessmentId],
			[NotificationReceivedDate])
VALUES (NEWID(),
		@NotificationAssessmentId,
		@NotificationCreateDate);

INSERT [Notification].[FinancialGuaranteeCollection]
		([Id],
		 [NotificationId])
VALUES (NEWID(),
		@NotificationId);

INSERT [Notification].[EntryExitCustomsSelection]
		(Id,
		[Entry],
		[Exit],
		TransportRouteId)
VALUES (NEWID(),
		0,
		0,
		@TransportRouteId);

END
