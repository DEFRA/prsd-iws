CREATE TABLE [Notification].[MovementPackagingInfo]
(
	[MovementId] UNIQUEIDENTIFIER NOT NULL,
	[PackagingInfoId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Data_MovementPackagingInfo] PRIMARY KEY CLUSTERED ([MovementId], [PackagingInfoId] ASC),
    CONSTRAINT [FK_Data_Movement] FOREIGN KEY ([MovementId]) REFERENCES [Notification].[Movement] (Id),
	CONSTRAINT [FK_Data_PackagingInfo] FOREIGN KEY ([PackagingInfoId]) REFERENCES [Notification].[PackagingInfo] (Id)
)