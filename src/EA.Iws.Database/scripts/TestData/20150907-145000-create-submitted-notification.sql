DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @NotificationId UNIQUEIDENTIFIER = '6C8DEFE2-5469-40FF-8EDA-19C1FE2AE485D';

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
        [isrecoverypercentagedataprovidedbyimporter],
        [wastegenerationprocess],
        [iswastegenerationprocessattached])
VALUES (@NotificationId,
        @UserId,
        1,
        1,
        N'GB 0001 002500',
        Cast(N'2015-07-13 18:31:30.0000000' AS DATETIME2),
        N'Recycling at advanced facility',
        0,
        NULL,
        1,
        NULL,
        1)

INSERT [Notification].[MeansOfTransport]
	   ([Id],
	    [NotificationId],
		[MeansOfTransport])
VALUES (N'6A872075-BD0B-4B69-9395-BBD12B0BA2FA',
		@NotificationId,
		N'R;S;R;A;R');

INSERT [Notification].[FacilityCollection] (
	[Id],
	[NotificationId],
	[AllFacilitiesPreconsented]
)
VALUES (
	N'24727DC6-FB47-4D00-AFA3-DBA89824C29B',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [FacilityCollectionId],
        [otherdescription])
VALUES (N'72fb7ec7-e4f7-4032-93a9-a4d40132ca72',
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
        N'24727DC6-FB47-4D00-AFA3-DBA89824C29B',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [FacilityCollectionId],
        [otherdescription])
VALUES (N'a13dd1a5-ac5a-4e8f-9481-a4d40132d4f2',
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
        N'John Bob',
        N'53-2225557777',
        N'53-3336669999',
        N'test@importer.de',
        N'24727DC6-FB47-4D00-AFA3-DBA89824C29B',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [notificationid],
        [otherdescription])
VALUES (N'089c64c9-6685-4b15-bdbf-a4d40132bb52',
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

INSERT [Notification].[ProducerCollection]
	   ([Id],
	    [NotificationId])
VALUES (N'A16D7EC0-DA99-4EA6-B549-924401E7B4D2',
		@NotificationId)

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
        [fullname],
        [telephone],
        [fax],
        [email],
        [producercollectionid],
        [otherdescription])
VALUES (N'bc7ebca5-4861-4f8f-8d0e-a4d40131fd42',
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
        N'James Teller',
        N'44-7778889999',
        N'44-1112223333',
        N'test@exporter.com',
        N'A16D7EC0-DA99-4EA6-B549-924401E7B4D2',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [producercollectionid],
        [otherdescription])
VALUES (N'23b8b9c5-35a0-4e62-ac84-a4d401321692',
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
        N'A16D7EC0-DA99-4EA6-B549-924401E7B4D2',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [notificationid],
        [otherdescription])
VALUES (N'5b58bc37-1a76-4640-9a23-a4d40131ef22',
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

INSERT [Notification].[shipmentinfo]
       ([id],
        [notificationid],
        [numberofshipments],
        [quantity],
        [units],
        [firstdate],
        [lastdate],
        [WillSelfEnterShipmentData])
VALUES (N'f300dc00-f20d-49d6-8871-a4d401343222',
        @NotificationId,
        520,
        Cast(10.0 AS DECIMAL(18, 4)),
        3,
        Cast(N'2015-09-01' AS DATE),
        Cast(N'2016-08-27' AS DATE),
        1)

INSERT [Notification].[CarrierCollection]
	   ([Id],
	    [NotificationId])
VALUES (N'5211F9A0-FA1B-46B4-BCC6-F43E03AB5310',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [otherdescription])
VALUES (N'26397716-26fc-451b-94e3-a4d401336472',
        N'Carrier',
        N'5211F9A0-FA1B-46B4-BCC6-F43E03AB5310',
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
        [fullname],
        [telephone],
        [fax],
        [email],
        [otherdescription])
VALUES (N'5a46baaa-d5dd-4c21-aac0-a4d401337a22',
        N'Carrier Two',
        N'5211F9A0-FA1B-46B4-BCC6-F43E03AB5310',
        2,
        N'CAR98765',
        NULL,
        N'1 Bridge Lane',
        N'High Street',
        N'Send',
        N'GU22 1AA',
        N'Surrey',
        N'United Kingdom',
        N'Michael Murray',
        N'44-4445556666',
        N'44-1112223333',
        N'test@carrier.com',
        NULL)

INSERT [Notification].[packaginginfo]
       ([id],
        [packagingtype],
        [otherdescription],
        [notificationid])
VALUES (N'21393bf2-8cba-4d09-9fdb-a4d401338cc2',
        4,
        NULL,
        @NotificationId)

INSERT [Notification].[packaginginfo]
       ([id],
        [packagingtype],
        [otherdescription],
        [notificationid])
VALUES (N'916bd103-265b-49bf-a619-a4d401338cc2',
        5,
        NULL,
        @NotificationId)

INSERT INTO [Notification].[TransportRoute]
([Id], [NotificationId])
VALUES ('31393bf2-8cba-4d09-9fdb-a4d401338eee', 
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
VALUES (N'0990a856-f001-45fe-a3cd-a4d40133a0d2',
        '31393bf2-8cba-4d09-9fdb-a4d401338eee',
        @CountryId,
        @CAId,
        @EntryId)

SELECT @CountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'Germany';

SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'DE023';

SELECT @ExitId = id
FROM   [Notification].[entryorexitpoint]
WHERE  [name] = 'Aachen';

INSERT [Notification].[stateofimport]
       ([id],
        [TransportRouteId],
        [countryid],
        [competentauthorityid],
        [entrypointid])
VALUES (N'a7e76c08-1939-4e56-88d8-a4d40133bd22',
        '31393bf2-8cba-4d09-9fdb-a4d401338eee',
        @CountryId,
        @CAId,
        @ExitId)

SELECT @CountryId = id
FROM   [Lookup].[country]
WHERE  [name] = 'France';

SELECT @CAId = id
FROM   [Lookup].[competentauthority]
WHERE  [code] = 'F';

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
VALUES (N'c0fb971c-c7f1-490a-a7b1-a4d40133db92',
        '31393bf2-8cba-4d09-9fdb-a4d401338eee',
        @CountryId,
        @CAId,
        @EntryId,
        @ExitId,
        1)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (N'477e8c48-478a-4f76-9417-a4d40132e6b2',
        @NotificationId,
        2)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (N'eef1eb16-8bc1-4af0-957b-a4d40132e6b2',
        @NotificationId,
        3)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (N'e9f2a0ac-b776-4c78-b1ff-a4d40132e6b2',
        @NotificationId,
        1)

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
VALUES (N'd8797ce3-c58f-4faf-9db9-a4d401346412',
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
VALUES (N'1f67c730-4bd2-43fe-8a17-a4d401346412',
        N'Plastics',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        2)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'c780661f-853d-4a5a-8c05-a4d401346412',
        N'Wood',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        4)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'a748dbf7-2b70-4e27-8ca1-a4d401346412',
        N'Paper',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        1)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'e095e4d2-f908-48ca-948e-a4d401346412',
        N'Textiles',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        5)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'44040e45-2f23-4cd8-a717-a4d401346412',
        N'Metals',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        6)

INSERT [Notification].[physicalcharacteristicsinfo]
       ([id],
        [physicalcharacteristictype],
        [otherdescription],
        [notificationid])
VALUES (N'52d690f1-7002-49ff-8f13-a4d401348692',
        4,
        NULL,
        @NotificationId)

INSERT [Notification].[physicalcharacteristicsinfo]
       ([id],
        [physicalcharacteristictype],
        [otherdescription],
        [notificationid])
VALUES (N'2210c309-8429-443c-ac7f-a4d401348692',
        3,
        NULL,
        @NotificationId)

INSERT [Notification].[technologyemployed]
       ([id],
        [annexprovided],
        [details],
        [notificationid],
        [furtherdetails])
VALUES (N'272e1409-ebc2-439b-87b5-a4d40132ebc2',
        0,
        'Electrolysis',
        @NotificationId,
        'A cathode and anode are used to separate recoverable materials from an ionic solution of waste.')

DECLARE @WasteCodeId UNIQUEIDENTIFIER;

INSERT [Notification].[wastecodeinfo]
       ([id],
        [wastecodeid],
        [customcode],
        [notificationid],
		[codetype])
VALUES (N'9e23dda3-283b-436a-98f7-a4d40134b272',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'A1030'),
        NULL,
        @NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'A1030')),

		(N'cd07b7de-2aea-4c67-9bb9-a4d40134b272',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = '01 05 04'),
        NULL,
        @NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = '01 05 04')),

		(N'59566772-1305-4f6c-861d-a4d40134cf12',
        NULL,
        N'1561231',
        @NotificationId,
		8),

		(N'377581f6-e570-4fa6-979d-a4d40134cf12',
		NULL,
        N'Iron filings',
        @NotificationId,
		7),
				
		(N'360581f6-e570-4fa6-979d-a4d40134cf12',
		NULL,
        N'GB01',
        @NotificationId,
		9),

		(N'367581f6-e570-4fa6-979d-a4d40134cf12',
		NULL,
        N'XV72663',
        @NotificationId,
		10),

		(N'bd32d603-f5c6-46f9-99f4-a4d40134e392',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [Description] = 'Explosives'),
        NULL,
        @NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [Description] = 'Explosives')),

		(N'9a6aee8d-6e4a-47c9-9e98-a4d40134e392',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'Y1'),
        NULL,
        @NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'Y1')),

		(N'9e9d509d-a612-4974-a431-a4d40134e392',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'H1'),
        NULL,
        @NotificationId,
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'H1')),

		(N'8cbe99ba-dd85-4393-bcee-a4d40134f502',
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
VALUES (N'a8556283-753d-48fe-9714-a4d4013479e2',
        N'Chlorine',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        5)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'a8ac0c58-850a-44cd-99ae-a4d4013479e2',
        N'Moisture content',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        2)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'93bb0f25-ef6d-4f1c-ab0b-a4d4013479e2',
        N'Ash content',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        3)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'a6d0fce1-b876-4e15-aca1-a4d4013479e2',
        N'Net calorific value',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        1)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'f5daeffa-f220-4a51-b83b-a4d4013479e2',
        N'Heavy metals',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346412',
        4)

INSERT INTO [Notification].[notificationassessment]
            ([id],
             [notificationapplicationid],
             [status])
VALUES      ('9060B7DA-0D29-494D-8F9B-3E16C5361D47',
              @NotificationId,
              2 ) ;

INSERT INTO [Notification].[NotificationDates]
(
	[Id],
	[NotificationAssessmentId]
)
VALUES
(
	(SELECT Cast(Cast(Newid() AS BINARY(10))
                           + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
	'9060B7DA-0D29-494D-8F9B-3E16C5361D47'
)

INSERT INTO [Notification].[FinancialGuaranteeCollection]
			(
				[Id],
				[NotificationId]
			)
VALUES
			(
				(SELECT Cast(Cast(Newid() AS BINARY(10))
							   + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
				@NotificationId
			)

INSERT [Notification].[EntryExitCustomsSelection]
		(Id,
		[Entry],
		[Exit],
		TransportRouteId
		)
VALUES (NEWID(),
		0,
		0,
		'31393bf2-8cba-4d09-9fdb-a4d401338eee')