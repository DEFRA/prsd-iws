CREATE TABLE [Lookup].[ChemicalCompositionCategory]
(
	[Id] INT CONSTRAINT PK_ChemicalCompositionCategory PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[ChemicalCompositionCategory] ([Id], [Description])
VALUES	(0, 'Other'),
		(1, 'Paper'),
		(2, 'Plastics'),
		(3, 'Food'),
		(4, 'Wood'),
		(5, 'Textiles'),
		(6, 'Metals');
GO

ALTER TABLE [Notification].[WasteComposition]
ADD CONSTRAINT FK_NotificationWasteComposition_ChemicalCompositionType FOREIGN KEY ([ChemicalCompositionType]) REFERENCES [Lookup].[ChemicalCompositionCategory]([Id]);
GO

CREATE TABLE [Lookup].[ChemicalCompositionType]
(
	[Id] INT CONSTRAINT PK_ChemicalCompositionType PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[ChemicalCompositionType] ([Id], [Description])
VALUES	(1, 'Refuse derived fuel (RDF)'),
		(2, 'Solid recovered fuel (SRF)'),
		(3, 'Wood'),
		(4, 'Other');
GO

ALTER TABLE [Notification].[WasteType]
ADD CONSTRAINT FK_NotificationWasteType_ChemicalCompositionType FOREIGN KEY ([ChemicalCompositionType]) REFERENCES [Lookup].[ChemicalCompositionType]([Id]);
GO

CREATE TABLE [Lookup].[WasteInformationType]
(
	[Id] INT CONSTRAINT PK_WasteInformationType PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[WasteInformationType] ([Id], [Description])
VALUES	(1, 'Net calorific value (Megajoules per kg)'),
		(2, 'Moisture content'),
		(3, 'Ash content'),
		(4, 'Heavy metals (milligrams per kg)'),
		(5, 'Chlorine'),
		(6, 'Energy Efficiency');
GO

ALTER TABLE [Notification].[WasteAdditionalInformation]
ADD CONSTRAINT FK_NotificationWasteAdditionalInformation_WasteInformationType FOREIGN KEY ([WasteInformationType]) REFERENCES [Lookup].[WasteInformationType]([Id]);
GO

CREATE TABLE [Lookup].[PhysicalCharacteristicType]
(
	[Id] INT CONSTRAINT PK_PhysicalCharacteristicType PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[PhysicalCharacteristicType] ([Id], [Description])
VALUES	(1, 'Powdery/powder'),
		(2, 'Solid'),
		(3, 'Viscous/paste'),
		(4, 'Sludgy'),
		(5, 'Liquid'),
		(6, 'Gaseous'),
		(7, 'Other');
GO

ALTER TABLE [Notification].[PhysicalCharacteristicsInfo]
ADD CONSTRAINT FK_NotificationPhysicalCharacteristicsInfo_PhysicalCharacteristicsType FOREIGN KEY ([PhysicalCharacteristicType]) REFERENCES [Lookup].[PhysicalCharacteristicType]([Id]);
GO

CREATE TABLE [Lookup].[PackagingType]
(
	[Id] INT CONSTRAINT PK_PackagingType PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[PackagingType] ([Id], [Description])
VALUES	(1, 'Drum'),
		(2, 'Wooden barrel'),
		(3, 'Jerrican'),
		(4, 'Box'),
		(5, 'Bag'),
		(6, 'Composite packaging'),
		(7, 'Pressure receptacle'),
		(8, 'Bulk'),
		(9, 'Other');
GO

ALTER TABLE [Notification].[PackagingInfo]
ADD CONSTRAINT FK_NotificationPackagingInfo_PackagingType FOREIGN KEY ([PackagingType]) REFERENCES [Lookup].[PackagingType]([Id]);
GO


CREATE TABLE [Lookup].[NotificationStatus]
(
	[Id] INT CONSTRAINT PK_NotificationStatus PRIMARY KEY NOT NULL,
	[Description] NVARCHAR(64) NOT NULL
);
GO

INSERT INTO [Lookup].[NotificationStatus] ([Id], [Description])
VALUES	(1, 'Not submitted'),
		(2, 'Submitted'),
		(3, 'Notification received'),
		(4, 'In assessment'),
		(5, 'Ready to transmit'),
		(6, 'Transmitted'),
		(7, 'Acknowledged'),
		(8, 'Decision required'),
		(9, 'Withdrawn'),
		(10, 'Objected'),
		(11, 'Consented'),
		(12, 'Consent withdrawn');
GO

ALTER TABLE [Notification].[NotificationAssessment]
ADD CONSTRAINT FK_NotificationAssessment_Status FOREIGN KEY ([Status]) REFERENCES [Lookup].[NotificationStatus]([Id]);
GO

ALTER TABLE [Notification].[NotificationStatusChange]
ADD CONSTRAINT FK_NotificationStatusChange_Status FOREIGN KEY ([Status]) REFERENCES [Lookup].[NotificationStatus]([Id]);
GO