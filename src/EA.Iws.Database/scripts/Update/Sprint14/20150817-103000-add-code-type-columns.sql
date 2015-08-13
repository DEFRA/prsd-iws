CREATE TABLE [Lookup].[CodeType]
(
	[Id] INT CONSTRAINT PK_CodeType PRIMARY KEY,
	[Name] NVARCHAR(256) NOT NULL
);
GO

INSERT INTO [Lookup].[CodeType]
(
	[Id],
	[Name]
)
VALUES
(1, 'Basel'),
(2, 'OECD'),
(3, 'EWC'),
(4, 'Y-Code'),
(5, 'H-Code'),
(6, 'UN class'),
(7, 'Export Code'),
(8, 'Import Code'),
(9, 'Other Code'),
(10, 'Customs Code'),
(11, 'UN Number');
GO

ALTER TABLE [Lookup].[WasteCode]
ADD CONSTRAINT FK_WasteCode_CodeType FOREIGN KEY ([CodeType]) REFERENCES [Lookup].[CodeType]([Id]);
GO

ALTER TABLE [Business].[WasteCodeInfo]
ADD CodeType INT;
GO

UPDATE [Business].[WasteCodeInfo]
SET CodeType = (SELECT TOP 1 CodeType FROM [Lookup].[WasteCode] AS WC WHERE WC.Id = WasteCodeId);
GO

ALTER TABLE [Business].[WasteCodeInfo]
ALTER COLUMN [CodeType] INT NOT NULL

ALTER TABLE [Business].[WasteCodeInfo]
ADD CONSTRAINT FK_WasteCodeInfo_CodeType FOREIGN KEY ([CodeType]) REFERENCES [Lookup].[CodeType]([Id]);
GO

ALTER TABLE [Business].[WasteCodeInfo]
ALTER COLUMN [WasteCodeId] UNIQUEIDENTIFIER NULL;
GO

UPDATE [Business].[WasteCodeInfo]
SET IsNotApplicable = 0
WHERE IsNotApplicable IS NULL;
GO

ALTER TABLE [Business].[WasteCodeInfo]
ADD CONSTRAINT DF_WasteCodeInfo_IsNotApplicable DEFAULT 0 FOR [IsNotApplicable];
GO

ALTER TABLE [Business].[WasteCodeInfo]
ALTER COLUMN [IsNotApplicable] BIT NOT NULL;
GO

UPDATE	[Business].[WasteCodeInfo]
SET		WasteCodeId = NULL,
		IsNotApplicable = 1
WHERE	WasteCodeId IN 
(
		SELECT	Id 
		FROM	[Lookup].[WasteCode] 
		WHERE	Code = 'Not applicable' 
		OR		Code = 'Not Listed'
);
GO

DELETE
FROM	[Lookup].[WasteCode] 
WHERE	Code = 'Not applicable' 
OR		Code = 'Not Listed';
GO