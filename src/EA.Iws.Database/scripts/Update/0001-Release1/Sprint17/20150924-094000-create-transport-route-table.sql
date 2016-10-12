CREATE TABLE [Notification].[TransportRoute](
	[Id] uniqueidentifier CONSTRAINT [PK_TransportRoute] PRIMARY KEY NOT NULL,
	[NotificationId] uniqueidentifier CONSTRAINT FK_TransportRoute_Notification FOREIGN KEY REFERENCES [Notification].[Notification]([Id]) NOT NULL,
	[RowVersion] [timestamp] NOT NULL
	)
GO

INSERT INTO [Notification].[TransportRoute] (
	[Id],
	[NotificationId]
)
SELECT 
	(select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)) as [Id],
	[Id] as [NotificationId]
FROM 
	[Notification].[Notification]
GO

ALTER TABLE [Notification].[StateOfExport]
ADD [TransportRouteId] UNIQUEIDENTIFIER CONSTRAINT FK_StateOfExport_TransportRoute FOREIGN KEY REFERENCES [Notification].[TransportRoute]([Id]) NULL
GO

ALTER TABLE [Notification].[StateOfImport]
ADD [TransportRouteId] UNIQUEIDENTIFIER CONSTRAINT FK_StateOfImport_TransportRoute FOREIGN KEY REFERENCES [Notification].[TransportRoute]([Id]) NULL
GO

ALTER TABLE [Notification].[TransitState]
ADD [TransportRouteId] UNIQUEIDENTIFIER CONSTRAINT FK_TransitState_TransportRoute FOREIGN KEY REFERENCES [Notification].[TransportRoute]([Id]) NULL
GO

ALTER TABLE [Notification].[EntryCustomsOffice]
ADD [TransportRouteId] UNIQUEIDENTIFIER CONSTRAINT FK_EntryCustomsOffice_TransportRoute FOREIGN KEY REFERENCES [Notification].[TransportRoute]([Id]) NULL
GO

ALTER TABLE [Notification].[ExitCustomsOffice]
ADD [TransportRouteId] UNIQUEIDENTIFIER CONSTRAINT FK_ExitCustomsOffice_TransportRoute FOREIGN KEY REFERENCES [Notification].[TransportRoute]([Id]) NULL
GO

UPDATE SE
SET
	SE.[TransportRouteId] = TR.[Id]
FROM
	[Notification].[TransportRoute] TR
	INNER JOIN [Notification].[StateOfExport] SE on SE.[NotificationId] = TR.[NotificationId]
GO

UPDATE SI
SET
	SI.[TransportRouteId] = TR.[Id]
FROM
	[Notification].[TransportRoute] TR
	INNER JOIN [Notification].[StateOfImport] SI on SI.[NotificationId] = TR.[NotificationId]
GO

UPDATE TS
SET
	TS.[TransportRouteId] = TR.[Id]
FROM
	[Notification].[TransportRoute] TR
	INNER JOIN [Notification].[TransitState] TS on TS.[NotificationId] = TR.[NotificationId]
GO

UPDATE CO
SET
	CO.[TransportRouteId] = TR.[Id]
FROM
	[Notification].[TransportRoute] TR
	INNER JOIN [Notification].[EntryCustomsOffice] CO on CO.[NotificationId] = TR.[NotificationId]
GO

UPDATE CO
SET
	CO.[TransportRouteId] = TR.[Id]
FROM
	[Notification].[TransportRoute] TR
	INNER JOIN [Notification].[ExitCustomsOffice] CO on CO.[NotificationId] = TR.[NotificationId]
GO

ALTER TABLE [Notification].[StateOfExport]
DROP CONSTRAINT FK_StateOfExport_Notification

DROP INDEX [IX_StateOfExport_NotificationId] ON [Notification].[StateOfExport]

ALTER TABLE [Notification].[StateOfExport]
DROP COLUMN [NotificationId]
GO

ALTER TABLE [Notification].[StateOfImport]
DROP CONSTRAINT FK_StateOfImport_Notification

DROP INDEX [IX_StateOfImport_NotificationId] ON [Notification].[StateOfImport]

ALTER TABLE [Notification].[StateOfImport]
DROP COLUMN [NotificationId]
GO

ALTER TABLE [Notification].[TransitState]
DROP CONSTRAINT FK_TransitState_Notification

DROP INDEX [IX_TransitState_NotificationId] ON [Notification].[TransitState]

ALTER TABLE [Notification].[TransitState]
DROP COLUMN [NotificationId]
GO

ALTER TABLE [Notification].[EntryCustomsOffice]
DROP CONSTRAINT FK_EntryCustomsOffice_Notification

DROP INDEX [IX_EntryCustomsOffice_NotificationId] ON [Notification].[EntryCustomsOffice]

ALTER TABLE [Notification].[EntryCustomsOffice]
DROP COLUMN [NotificationId]
GO

ALTER TABLE [Notification].[ExitCustomsOffice]
DROP CONSTRAINT FK_ExitCustomsOffice_Notification

DROP INDEX [IX_ExitCustomsOffice_NotificationId] ON [Notification].[ExitCustomsOffice]

ALTER TABLE [Notification].[ExitCustomsOffice]
DROP COLUMN [NotificationId]
GO

ALTER TABLE [Notification].[StateOfExport]
ALTER COLUMN [TransportRouteId] UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [Notification].[StateOfImport]
ALTER COLUMN [TransportRouteId] UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [Notification].[TransitState]
ALTER COLUMN [TransportRouteId] UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [Notification].[EntryCustomsOffice]
ALTER COLUMN [TransportRouteId] UNIQUEIDENTIFIER NOT NULL

ALTER TABLE [Notification].[ExitCustomsOffice]
ALTER COLUMN [TransportRouteId] UNIQUEIDENTIFIER NOT NULL

GO