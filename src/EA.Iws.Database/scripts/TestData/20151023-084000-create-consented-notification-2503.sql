DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @NotificationId UNIQUEIDENTIFIER = 'ceb03ae8-2792-4dfc-8f87-6b2384f62503';

SELECT @UserId = id
FROM   [Identity].[aspnetusers]
WHERE  [email] = 'sunily@sfwltd.co.uk'

INSERT [Notification].[notification]
       ([id],
        [userid],
        [notificationtype],
        [competentauthority],
        [notificationnumber],
        [createddate],
        [reasonforexport],
        [hasspecialhandlingrequirements],
        [specialhandlingdetails],
        [meansoftransport],
        [isrecoverypercentagedataprovidedbyimporter],
        [wastegenerationprocess],
        [iswastegenerationprocessattached])
VALUES (@NotificationId,
        @UserId,
        1,
        1,
        N'GB 0001 002503',
        Cast(N'2015-10-10 09:00:00.0000000' AS DATETIME2),
        N'Use of advanced facilities',
        0,
        NULL,
        N'R;S;R;T',
        1,
        NULL,
        1)

INSERT [Notification].[FacilityCollection] (
	[Id],
	[NotificationId],
	[AllFacilitiesPreconsented]
)
VALUES (
	N'0315EF80-778F-48E0-A95C-6A53A3F3ABCB',
	@NotificationId,
	1
)

INSERT [Notification].[facility]
       ([id],
        [name],
        [isactualsiteoftreatment],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [FacilityCollectionId],
        [otherdescription])
VALUES (NEWID(),
        N'Waste Treatment Facility',
        1,
        1,
        N'45645684546',
        NULL,
        N'1 Rue Strasse',
        N'German Suburb',
        N'Dortmund',
        N'64151',
        N'Dortmund',
        N'Germany',
        N'Maxwell',
        N'Bob',
        N'53-2225557777',
        N'53-3336669999',
        N'test@importer.de',
        N'0315EF80-778F-48E0-A95C-6A53A3F3ABCB',
        NULL)

INSERT [Notification].[facility]
       ([id],
        [name],
        [isactualsiteoftreatment],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [FacilityCollectionId],
        [otherdescription])
VALUES (NEWID(),
        N'Scrap Waste Reclamation',
        0,
        2,
        N'IMPORTER123',
        NULL,
        N'1 Normal Strasse',
        N'Near XYZ',
        N'Dortmund',
        N'654451',
        N'Dortmund',
        N'Germany',
        N'John',
        N'Bob',
        N'53-2225557777',
        N'53-3336669999',
        N'test@importer.de',
        N'0315EF80-778F-48E0-A95C-6A53A3F3ABCB',
        NULL)

INSERT [Notification].[importer]
       ([id],
        [name],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [notificationid],
        [otherdescription])
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
        N'John',
        N'Bob',
        N'44-2225557777',
        N'44-3336669999',
        N'test@importer.com',
        @NotificationId,
        NULL)

INSERT [Notification].[producer]
       ([id],
        [name],
        [issiteofexport],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [countryid],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [notificationid],
        [otherdescription])
VALUES (NEWID(),
        N'Metal Products Ltd',
        1,
        3,
        N'Not applicable',
        NULL,
        N'Station Approach',
        N'Opp. TESCO',
        N'Woking',
        N'GU22 7UY',
        N'Surrey',
        N'United Kingdom',
        NULL,
        N'James',
        N'Teller',
        N'44-7778889999',
        N'44-1112223333',
        N'test@exporter.com',
        @NotificationId,
        NULL)

INSERT [Notification].[producer]
       ([id],
        [name],
        [issiteofexport],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [countryid],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [notificationid],
        [otherdescription])
VALUES (NEWID(),
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
        NULL,
        N'John',
        N'Smith',
        N'44-7778889999',
        N'44-1112223333',
        N'test@producer.com',
        @NotificationId,
        NULL)

INSERT [Notification].[exporter]
       ([id],
        [name],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [notificationid],
        [otherdescription])
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
        N'John',
        N'Smith',
        N'44-7778889999',
        N'44-1112223333',
        N'test@exporter.com',
        @NotificationId,
        NULL)

INSERT [Notification].[shipmentinfo]
       ([id],
        [notificationid],
        [numberofshipments],
        [quantity],
        [units],
        [firstdate],
        [lastdate])
VALUES (NEWID(),
        @NotificationId,
        520,
        Cast(25000.0000 AS DECIMAL(18, 4)),
        3,
        Cast(N'2015-09-01' AS DATE),
        Cast(N'2016-08-27' AS DATE))

INSERT [Notification].[CarrierCollection]
	   ([Id],
	    [NotificationId])
VALUES (N'0B23F00D-AC86-4C67-807F-1F0AD07EC96E',
		@NotificationId)

INSERT [Notification].[carrier]
       ([id],
        [name],
        [carriercollectionid],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [otherdescription])
VALUES (NEWID(),
        N'Carrier',
        N'0B23F00D-AC86-4C67-807F-1F0AD07EC96E',
        1,
        N'CARRIER12345',
        NULL,
        N'Business House',
        N'Reading Street',
        N'Guildford',
        N'GU21 5EM',
        N'Surrey',
        N'United Kingdom',
        N'Karen',
        N'Murrey',
        N'44-4445556666',
        N'44-1112223333',
        N'test@carrier.com',
        NULL)

INSERT [Notification].[carrier]
       ([id],
        [name],
        [carriercollectionid],
        [type],
        [registrationnumber],
        [additionalregistrationnumber],
        [address1],
        [address2],
        [townorcity],
        [postalcode],
        [region],
        [country],
        [firstname],
        [lastname],
        [telephone],
        [fax],
        [email],
        [otherdescription])
VALUES (NEWID(),
        N'Carrier Two',
        N'0B23F00D-AC86-4C67-807F-1F0AD07EC96E',
        2,
        N'CAR98765',
        NULL,
        N'1 Bridge Lane',
        N'High Street',
        N'Send',
        N'GU22 1AA',
        N'Surrey',
        N'United Kingdom',
        N'Michael',
        N'Murray',
        N'44-4445556666',
        N'44-1112223333',
        N'test@carrier.com',
        NULL)

INSERT [Notification].[packaginginfo]
       ([id],
        [packagingtype],
        [otherdescription],
        [notificationid])
VALUES (NEWID(),
        4,
        NULL,
        @NotificationId)

INSERT [Notification].[packaginginfo]
       ([id],
        [packagingtype],
        [otherdescription],
        [notificationid])
VALUES (NEWID(),
        5,
        NULL,
        @NotificationId)

DECLARE @TransportRouteId UNIQUEIDENTIFIER = NEWID();
INSERT INTO [Notification].[TransportRoute]
([Id], [NotificationId])
VALUES (@TransportRouteId, 
		@NotificationId)

DECLARE @CountryId UNIQUEIDENTIFIER;

SELECT @CountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'United Kingdom';

DECLARE @CAId UNIQUEIDENTIFIER;

SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'GB01';

DECLARE @EntryId UNIQUEIDENTIFIER;
DECLARE @ExitId UNIQUEIDENTIFIER;

SELECT @EntryId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Dover';

INSERT [Notification].[stateofexport]
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
WHERE  [name] = 'Germany';

SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'DE 023';

SELECT @ExitId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Aachen';

INSERT [Notification].[stateofimport]
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
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'FR999';

SELECT @EntryId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Calais';

SELECT @ExitId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Lille';

INSERT [Notification].[transitstate]
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

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (NEWID(),
        @NotificationId,
        2)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (NEWID(),
        @NotificationId,
        3)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (NEWID(),
        @NotificationId,
        1)

DECLARE @WasteTypeId UNIQUEIDENTIFIER = NEWID();		
		
INSERT [Notification].[wastetype]
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

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (NEWID(),
        N'Food',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        @WasteTypeId,
        3)

INSERT [Notification].[wastecomposition]
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

INSERT [Notification].[wastecomposition]
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

INSERT [Notification].[wastecomposition]
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

INSERT [Notification].[wastecomposition]
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

INSERT [Notification].[wastecomposition]
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

INSERT [Notification].[physicalcharacteristicsinfo]
       ([id],
        [physicalcharacteristictype],
        [otherdescription],
        [notificationid])
VALUES (NEWID(),
        4,
        NULL,
        @NotificationId)

INSERT [Notification].[physicalcharacteristicsinfo]
       ([id],
        [physicalcharacteristictype],
        [otherdescription],
        [notificationid])
VALUES (NEWID(),
        3,
        NULL,
        @NotificationId)

INSERT [Notification].[technologyemployed]
       ([id],
        [annexprovided],
        [details],
        [notificationid],
        [furtherdetails])
VALUES (NEWID(),
        0,
        'Electrolysis',
        @NotificationId,
        'A cathode and anode are used to separate recoverable materials from an ionic solution of waste.')

INSERT [Notification].[wastecodeinfo]
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

INSERT [Notification].[wasteadditionalinformation]
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

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (NEWID(),
        N'Moisture content',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        @WasteTypeId,
        2)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (NEWID(),
        N'Ash content',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        @WasteTypeId,
        3)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (NEWID(),
        N'Net calorific value (Megajoules per kg)',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        @WasteTypeId,
        1)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (NEWID(),
        N'Heavy metals (Megajoules per kg)',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        @WasteTypeId,
        4)

DECLARE @NotificationAssessmentId UNIQUEIDENTIFIER = NEWID();

INSERT INTO [Notification].[notificationassessment]
            ([id],
             [notificationapplicationid],
             [status])
VALUES      (@NotificationAssessmentId,
              @NotificationId,
              10 ) ;

INSERT INTO [Notification].[NotificationDates]
(
	[Id],
	[NotificationAssessmentId],
	[NotificationReceivedDate],
    [PaymentReceivedDate],
    [CommencementDate],
    [CompleteDate],
    [TransmittedDate],
    [AcknowledgedDate],
    [NameOfOfficer]
)
VALUES
(
	(SELECT Cast(Cast(Newid() AS BINARY(10))
                           + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
	@NotificationAssessmentId,
	'2015-10-20',
	'2015-10-20',
	'2015-10-21',
	'2015-10-21',
	'2015-10-22',
	'2015-10-23',
	'Jane'
)

INSERT INTO [Notification].[Consent]
(
	[Id],
	[From],
	[To],
	[Conditions],
	[UserId],
	[NotificationApplicationId]
)
VALUES
(
	(SELECT Cast(Cast(Newid() AS BINARY(10))
                           + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
	'2015-10-23',
	'2016-10-22',
	'Let me win at chess',
	(SELECT [Id] FROM [Identity].[AspNetUsers] WHERE [Email] LIKE 'superuser@environment-agency.gov.uk'),
	@NotificationId
)


INSERT INTO [Notification].[FinancialGuarantee]
(
	[Id],
	[Status],
	[ReceivedDate],
	[CompletedDate],
	[CreatedDate],
	[NotificationApplicationId],
	[DecisionDate],
	[ApprovedFrom],
	[ApprovedTo],
	[ActiveLoadsPermitted]
)
VALUES
(
	(SELECT Cast(Cast(Newid() AS BINARY(10))
					+ Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
	4,
	'2015-10-16',
	'2015-10-16',
	GETDATE(),
	@NotificationId,
	'2015-10-23',
	'2015-10-23',
	'2017-10-23',
	520
)
