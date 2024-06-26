DECLARE @UserId UNIQUEIDENTIFIER;

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
VALUES (N'fb6b9288-a487-41d2-8601-a4d4013148aa',
        @UserId,
        1,
        1,
        N'GB 0001 003000',
        Cast(N'2015-07-13 18:31:30.0000000' AS DATETIME2),
        N'Reason of export is my business policy',
        0,
        NULL,
        1,
        NULL,
        1)

INSERT [Notification].[MeansOfTransport]
	   ([Id],
	    [NotificationId],
		[MeansOfTransport])
VALUES (N'63918474-4D9F-4255-89C6-AE76B90D7167',
		N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		N'R');

INSERT [Notification].[FacilityCollection] (
	[Id],
	[NotificationId],
	[AllFacilitiesPreconsented]
)
VALUES (
	N'caf73b13-5fa3-4e58-8d74-1666de3369fb',
	N'fb6b9288-a487-41d2-8601-a4d4013148aa',
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
VALUES (N'72fb7ec7-e4f7-4032-93a9-a4d40132ca71',
        N'Importer Facility',
        1,
        1,
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
        N'caf73b13-5fa3-4e58-8d74-1666de3369fb',
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
VALUES (N'a13dd1a5-ac5a-4e8f-9481-a4d40132d4fc',
        N'Importer Facility1',
        0,
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
        N'caf73b13-5fa3-4e58-8d74-1666de3369fb',
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
VALUES (N'089c64c9-6685-4b15-bdbf-a4d40132bb5f',
        N'Importer',
        3,
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
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
        NULL)

INSERT [Notification].[ProducerCollection]
	   ([Id],
	    [NotificationId])
VALUES (N'B2176915-9BD3-4BE0-8D56-2E21A73025EA',
		N'fb6b9288-a487-41d2-8601-a4d4013148aa')

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
VALUES (N'bc7ebca5-4861-4f8f-8d0e-a4d40131fd4f',
        N'Exporter',
        1,
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
        N'test@exporter.com',
        N'B2176915-9BD3-4BE0-8D56-2E21A73025EA',
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
VALUES (N'23b8b9c5-35a0-4e62-ac84-a4d40132169c',
        N'New Producer',
        0,
        2,
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
        N'B2176915-9BD3-4BE0-8D56-2E21A73025EA',
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
VALUES (N'5b58bc37-1a76-4640-9a23-a4d40131ef26',
        N'Exporter',
        3,
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
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
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
VALUES (N'f300dc00-f20d-49d6-8871-a4d401343224',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
        99999,
        Cast(10.0 AS DECIMAL(18, 4)),
        3,
        Cast(N'2015-12-31' AS DATE),
        Cast(N'2016-12-31' AS DATE),
        1)

INSERT [Notification].[CarrierCollection]
	   ([Id],
	    [NotificationId])
VALUES (N'B9543BA8-765E-4203-AC3F-596AE5D07390',
		N'fb6b9288-a487-41d2-8601-a4d4013148aa')

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
VALUES (N'26397716-26fc-451b-94e3-a4d401336477',
        N'Carrier',
        N'B9543BA8-765E-4203-AC3F-596AE5D07390',
        1,
        N'CARRIER12345',
        NULL,
        N'Near Moon',
        N'Opp Sun',
        N'Woking',
        N'GU22 7UY',
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
VALUES (N'5a46baaa-d5dd-4c21-aac0-a4d401337a29',
        N'Carrier Two',
        N'B9543BA8-765E-4203-AC3F-596AE5D07390',
        3,
        N'CAR98765',
        NULL,
        N'Near Moon',
        N'Opp Sun',
        N'Woking',
        N'GU22 7UY',
        N'Surrey',
        N'United Kingdom',
        N'Karen Murrey',
        N'44-4445556666',
        N'44-1112223333',
        N'test@carrier.com',
        NULL)

INSERT [Notification].[packaginginfo]
       ([id],
        [packagingtype],
        [otherdescription],
        [notificationid])
VALUES (N'21393bf2-8cba-4d09-9fdb-a4d401338ccc',
        4,
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa')

INSERT [Notification].[packaginginfo]
       ([id],
        [packagingtype],
        [otherdescription],
        [notificationid])
VALUES (N'916bd103-265b-49bf-a619-a4d401338ccc',
        5,
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa')

INSERT INTO [Notification].[TransportRoute]
([Id], [NotificationId])
VALUES ('21393bf2-8cba-4d09-9fdb-a4d401338eee', 
		'fb6b9288-a487-41d2-8601-a4d4013148aa')

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
VALUES (N'0990a856-f001-45fe-a3cd-a4d40133a0dc',
        N'21393bf2-8cba-4d09-9fdb-a4d401338eee',
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
VALUES (N'a7e76c08-1939-4e56-88d8-a4d40133bd27',
        N'21393bf2-8cba-4d09-9fdb-a4d401338eee',
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
VALUES (N'c0fb971c-c7f1-490a-a7b1-a4d40133db93',
        N'21393bf2-8cba-4d09-9fdb-a4d401338eee',
        @CountryId,
        @CAId,
        @EntryId,
        @ExitId,
        1)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (N'477e8c48-478a-4f76-9417-a4d40132e6b4',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
        2)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (N'eef1eb16-8bc1-4af0-957b-a4d40132e6b4',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
        3)

INSERT [Notification].[operationcodes]
       ([id],
        [notificationid],
        [operationcode])
VALUES (N'e9f2a0ac-b776-4c78-b1ff-a4d40132e6b4',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
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
VALUES (N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        3,
        NULL,
        N'Wooden blocks',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
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
VALUES (N'0a6e0fc3-2d2d-4612-8829-a4d401346416',
        N'Food',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        3)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'1f67c730-4bd2-43fe-8a17-a4d401346416',
        N'Plastics',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        2)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'c780661f-853d-4a5a-8c05-a4d401346416',
        N'Wood',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        4)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'a748dbf7-2b70-4e27-8ca1-a4d401346416',
        N'Paper',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        1)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'e095e4d2-f908-48ca-948e-a4d401346416',
        N'Textiles',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        5)

INSERT [Notification].[wastecomposition]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [chemicalcompositiontype])
VALUES (N'44040e45-2f23-4cd8-a717-a4d401346416',
        N'Metals',
        Cast(1.00 AS DECIMAL(5, 2)),
        Cast(3.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        6)

INSERT [Notification].[physicalcharacteristicsinfo]
       ([id],
        [physicalcharacteristictype],
        [otherdescription],
        [notificationid])
VALUES (N'52d690f1-7002-49ff-8f13-a4d40134869d',
        4,
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa')

INSERT [Notification].[physicalcharacteristicsinfo]
       ([id],
        [physicalcharacteristictype],
        [otherdescription],
        [notificationid])
VALUES (N'2210c309-8429-443c-ac7f-a4d40134869d',
        3,
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa')

INSERT [Notification].[technologyemployed]
       ([id],
        [annexprovided],
        [details],
        [notificationid],
        [furtherdetails])
VALUES (N'272e1409-ebc2-439b-87b5-a4d40132ebc0',
        0,
        'Test details less than 70 chars',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
        'This is furthe details for technology employed')

DECLARE @WasteCodeId UNIQUEIDENTIFIER;

INSERT [Notification].[wastecodeinfo]
       ([id],
        [wastecodeid],
        [customcode],
        [notificationid],
		[codetype])
VALUES (N'9e23dda3-283b-436a-98f7-a4d40134b271',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'A1030'),
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'A1030')),

		(N'cd07b7de-2aea-4c67-9bb9-a4d40134b271',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = '01 05 04'),
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = '01 05 04')),

		(N'59566772-1305-4f6c-861d-a4d40134cf11',
        NULL,
        N'1561231',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		8),

		(N'377581f6-e570-4fa6-979d-a4d40134cf11',
		NULL,
        N'Iron filings',
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		7),

		(N'297BDB16-B4D2-4F40-B7FC-1766719094FD',
		NULL,
		N'TESTCUSTOMSCODE',
		N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		10),

		(N'7E6638F0-77FE-4FA5-B51E-1A748B638742',
		NULL,
		N'OTHERCODE',
		N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		9),

		(N'bd32d603-f5c6-46f9-99f4-a4d40134e397',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [Description] = 'Explosives'),
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [Description] = 'Explosives')),

		(N'9a6aee8d-6e4a-47c9-9e98-a4d40134e397',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'Y1'),
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'Y1')),

		(N'9e9d509d-a612-4974-a431-a4d40134e397',
        (SELECT TOP 1 id FROM [Lookup].[WasteCode] WHERE [code] = 'H1'),
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'H1')),

		(N'8cbe99ba-dd85-4393-bcee-a4d40134f506',
        (SELECT TOP 1 Id FROM [Lookup].[WasteCode] WHERE [code] = 'UN 0004'),
        NULL,
        N'fb6b9288-a487-41d2-8601-a4d4013148aa',
		(SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] WHERE [code] = 'UN 0004'));

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'a8556283-753d-48fe-9714-a4d4013479ee',
        N'Chlorine',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        5)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'a8ac0c58-850a-44cd-99ae-a4d4013479ee',
        N'Moisture content',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        2)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'93bb0f25-ef6d-4f1c-ab0b-a4d4013479ee',
        N'Ash content',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        3)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'a6d0fce1-b876-4e15-aca1-a4d4013479ee',
        N'Net calorific value',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        1)

INSERT [Notification].[wasteadditionalinformation]
       ([id],
        [constituent],
        [minconcentration],
        [maxconcentration],
        [wastetypeid],
        [wasteinformationtype])
VALUES (N'f5daeffa-f220-4a51-b83b-a4d4013479ee',
        N'Heavy metals',
        Cast(2.00 AS DECIMAL(5, 2)),
        Cast(5.00 AS DECIMAL(5, 2)),
        N'd8797ce3-c58f-4faf-9db9-a4d401346416',
        4)

INSERT INTO [Notification].[notificationassessment]
            ([id],
             [notificationapplicationid],
             [status])
VALUES      ( N'7A95C98C-087E-4C15-A034-CBDE43CF0B91',
              N'fb6b9288-a487-41d2-8601-a4d4013148aa',
              1 ) ;

INSERT INTO [Notification].[NotificationDates]
			([id],
			 [notificationassessmentid])
VALUES		( N'4752EC14-529F-4A24-AFE3-AF615B141817',
			  N'7A95C98C-087E-4C15-A034-CBDE43CF0B91');

INSERT INTO [Notification].[FinancialGuaranteeCollection]
			(
				[Id],
				[NotificationId]
			)
VALUES
			(
				(SELECT Cast(Cast(Newid() AS BINARY(10))
							   + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
				N'fb6b9288-a487-41d2-8601-a4d4013148aa'
			)
