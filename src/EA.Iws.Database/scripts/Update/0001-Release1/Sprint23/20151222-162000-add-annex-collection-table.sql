CREATE TABLE [Notification].[AnnexCollection]
(
	[Id]	UNIQUEIDENTIFIER NOT NULL CONSTRAINT PK_AnnexCollection PRIMARY KEY,
	[NotificationId] UNIQUEIDENTIFIER NOT NULL CONSTRAINT FK_AnnexCollection_NotificationId FOREIGN KEY REFERENCES [Notification].[Notification]([Id]),
	[ProcessOfGenerationId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_AnnexCollection_ProcessOfGeneration FOREIGN KEY REFERENCES [FileStore].[File]([Id]),
	[WasteCompositionId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_AnnexCollection_WasteComposition FOREIGN KEY REFERENCES [FileStore].[File]([Id]),
	[TechnologyEmployedId] UNIQUEIDENTIFIER NULL CONSTRAINT FK_AnnexCollection_TechnologyEmployed FOREIGN KEY REFERENCES [FileStore].[File]([Id]),
	[RowVersion] ROWVERSION NOT NULL
);
GO

INSERT INTO [Notification].[AnnexCollection]([Id], [NotificationId])
SELECT	(SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER)),
		 Id
FROM	[Notification].[Notification]
WHERE	Id NOT IN (SELECT NotificationId FROM [Notification].[AnnexCollection]);
GO