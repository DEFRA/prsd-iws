CREATE TABLE [Draft].[BulkUpload]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [NotificationId] UNIQUEIDENTIFIER NOT NULL, 
    [CreatedDate] DATE NULL, 
    [CreatedBy] NVARCHAR(256) NULL,
	[RowVersion] [timestamp] NOT NULL,
    CONSTRAINT [PK_Draft_BulkUpload] PRIMARY KEY CLUSTERED ([Id] ASC)
)

GO

CREATE TABLE [Draft].[Movement]
(
	[Id] UNIQUEIDENTIFIER NOT NULL, 
    [BulkUploadId] UNIQUEIDENTIFIER NOT NULL, 
	[NotificationNumber] NVARCHAR(100) NULL,
	[ShipmentNumber] INT NULL,
    [Quantity] DECIMAL(18, 4) NULL, 
    [Units] INT NULL, 
    [Date] DATE NULL, 
	[RowVersion] [timestamp] NOT NULL,
    CONSTRAINT [PK_Draft_Movement] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Draft_Movement_BulkUpload] FOREIGN KEY ([BulkUploadId]) REFERENCES [Draft].[BulkUpload] (Id)
)

GO

CREATE TABLE [Draft].[PackagingInfo]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [DraftMovementId] UNIQUEIDENTIFIER NOT NULL,
	[PackagingType] INT NOT NULL,
	[OtherDescription] NVARCHAR(100) NULL,
	[RowVersion] [timestamp] NOT NULL,
	CONSTRAINT [PK_Draft_PackagingInfo] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_Draft_Movement_PackagingInfo] FOREIGN KEY ([DraftMovementId]) REFERENCES [Draft].[Movement] (Id),
	CONSTRAINT [FK_Draft_PackagingType_PackagingInfo] FOREIGN KEY ([PackagingType]) REFERENCES [Lookup].[PackagingType] (Id)
)