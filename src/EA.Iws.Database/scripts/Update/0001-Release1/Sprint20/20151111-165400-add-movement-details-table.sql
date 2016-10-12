CREATE TABLE [Notification].[MovementDetails](
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[MovementId] UNIQUEIDENTIFIER NOT NULL,
	[Quantity] DECIMAL(18,4) NOT NULL,
	[Unit] INT NOT NULL,
	[NumberOfPackages] INT NOT NULL,
	[RowVersion] ROWVERSION NOT NULL,
	CONSTRAINT [PK_MovementDetails] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_MovementDetails_Movement] FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement]([Id]),
	CONSTRAINT [FK_MovementDetails_ShipmentQuantityUnit] FOREIGN KEY ([Unit]) REFERENCES [Lookup].[ShipmentQuantityUnit]([Id])
);
GO

DECLARE @DeletedMovements TABLE ([Id] UNIQUEIDENTIFIER)
INSERT INTO @DeletedMovements
SELECT [Id] FROM [Notification].[Movement]
WHERE [Status] = 1
OR [Date] IS NULL 
OR [Status] IS NULL 
OR [Quantity] IS NULL
OR [Unit] IS NULL
OR [NumberOfPackages] IS NULL;

DELETE FROM [Notification].[MovementCarrier]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementPackagingInfo]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementReceipt]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementOperationReceipt]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[MovementStatusChange]
WHERE [MovementId] IN (SELECT [Id] FROM @DeletedMovements);

DELETE FROM [Notification].[Movement]
WHERE [Id] IN (SELECT [Id] FROM @DeletedMovements);
GO

INSERT INTO [Notification].[MovementDetails]
(
	[Id],
	[MovementId],
	[Quantity],
	[Unit],
	[NumberOfPackages]
)
SELECT
	(SELECT CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
	[Id],
	[Quantity],
	[Unit],
	[NumberOfPackages]
FROM [Notification].[Movement]
WHERE [Status] <> 1 OR [Status] IS NOT NULL;
GO

ALTER TABLE [Notification].[Movement]
DROP CONSTRAINT FK_NotificationUnit_ShipmentQuantityUnit;
GO

ALTER TABLE [Notification].[Movement]
DROP COLUMN [Quantity];
GO

ALTER TABLE [Notification].[Movement]
DROP COLUMN [Unit];
GO

ALTER TABLE [Notification].[Movement]
DROP COLUMN [NumberOfPackages];
GO

ALTER TABLE [Notification].[MovementCarrier]
ADD [MovementDetailsId] UNIQUEIDENTIFIER NULL;
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
ADD [MovementDetailsId] UNIQUEIDENTIFIER NULL;
GO

UPDATE MC
SET MC.[MovementDetailsId] = MD.[Id]
FROM 
	[Notification].[MovementCarrier] MC
	INNER JOIN [Notification].[MovementDetails] MD 
		ON MC.[MovementId] = MD.[MovementId];
GO

UPDATE MPI
SET MPI.[MovementDetailsId] = MD.[Id]
FROM 
	[Notification].[MovementPackagingInfo] MPI
	INNER JOIN [Notification].[MovementDetails] MD
		ON MPI.[MovementId] = MD.[MovementId];
GO

ALTER TABLE [Notification].[MovementCarrier]
DROP CONSTRAINT FK_MovementCarrier_Movement;
GO

ALTER TABLE [Notification].[MovementCarrier]
DROP COLUMN [MovementId];

ALTER TABLE [Notification].[MovementCarrier]
ALTER COLUMN [MovementDetailsId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[MovementCarrier]
ADD CONSTRAINT FK_MovementCarrier_MovementDetails FOREIGN KEY ([MovementDetailsId]) REFERENCES [Notification].[MovementDetails]([Id]);
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
DROP CONSTRAINT PK_Data_MovementPackagingInfo;
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
DROP CONSTRAINT FK_Data_Movement;
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
DROP CONSTRAINT FK_Data_PackagingInfo;
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
DROP COLUMN [MovementId];
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
ALTER COLUMN [MovementDetailsId] UNIQUEIDENTIFIER NOT NULL;
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
ADD CONSTRAINT PK_MovementPackagingInfo PRIMARY KEY CLUSTERED ([MovementDetailsId] ASC, [PackagingInfoId] ASC);
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
ADD CONSTRAINT FK_MovementPackagingInfo_MovementDetails FOREIGN KEY ([MovementDetailsId]) REFERENCES [Notification].[MovementDetails]([Id]);
GO

ALTER TABLE [Notification].[MovementPackagingInfo]
ADD CONSTRAINT FK_MovementPackagingInfo_PackagingInfo FOREIGN KEY ([PackagingInfoId]) REFERENCES [Notification].[PackagingInfo]([Id]);
GO