CREATE TABLE [Notification].[MovementRejection]
(
	[Id]				UNIQUEIDENTIFIER	NOT NULL CONSTRAINT [PK_MovementRejection] PRIMARY KEY,
    [MovementId]		UNIQUEIDENTIFIER	NOT NULL CONSTRAINT [FK_MovementRejection_Movement] FOREIGN KEY REFERENCES [Notification].[Movement]([Id]),
    [Date]				DATETIMEOFFSET		NOT NULL,
	[Reason]			NVARCHAR(2048)		NOT NULL,
	[FurtherDetails]	NVARCHAR(MAX)		NULL,
	[FileId]			UNIQUEIDENTIFIER	NULL,
    [RowVersion]		ROWVERSION			NOT NULL
);
GO

INSERT INTO [Notification].[MovementRejection]
([MovementId], [Date], [Reason])
SELECT	[MovementId],
		[Date],
		[RejectReason] 
FROM	[Notification].[MovementReceipt]
WHERE	[RejectReason] IS NOT NULL
AND		[Quantity] IS NULL;
GO

DELETE FROM [Notification].[MovementReceipt]
WHERE	[RejectReason] IS NOT NULL
AND		[Quantity] IS NULL;
GO