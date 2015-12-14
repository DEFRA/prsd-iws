CREATE TABLE [ImportNotification].[TransportRoute](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [PK_ImportNotificationTransportRoute] PRIMARY KEY,
	[ImportNotificationId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationTransportRoute_ImportNotification
		FOREIGN KEY REFERENCES [ImportNotification].[Notification]([Id]),
	[RowVersion] [timestamp] NOT NULL
);
GO

CREATE TABLE [ImportNotification].[TransitState](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT [PK_ImportNotificationTransitState] PRIMARY KEY,
	[TransportRouteId] [uniqueidentifier] NOT NULL CONSTRAINT [FK_ImportNotificationTransitState_TransportRoute]
		FOREIGN KEY REFERENCES [ImportNotification].[TransportRoute]([Id]),
	[CountryId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationTransitState_Country
		FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[CompetentAuthorityId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationTransitState_CompetentAuthority
		FOREIGN KEY REFERENCES [Lookup].[CompetentAuthority]([Id]),
	[EntryPointId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationTransitState_EntryPoint
		FOREIGN KEY REFERENCES [Notification].[EntryOrExitPoint]([Id]),
	[ExitPointId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationTransitState_ExitPoint
		FOREIGN KEY REFERENCES [Notification].[EntryOrExitPoint]([Id]),
	[OrdinalPosition] [int] NOT NULL,
	[RowVersion] [timestamp] NOT NULL
);
GO

CREATE TABLE [ImportNotification].[StateOfExport](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationStateOfExport PRIMARY KEY,
	[TransportRouteId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfExport_TransportRoute
		FOREIGN KEY REFERENCES [ImportNotification].[TransportRoute]([Id]),
	[CountryId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfExport_Country
		FOREIGN KEY REFERENCES [Lookup].[Country]([Id]),
	[CompetentAuthorityId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfExport_CompetentAuthority
		FOREIGN KEY REFERENCES [Lookup].[CompetentAuthority]([Id]),
	[ExitPointId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfExport_ExitPoint
		FOREIGN KEY REFERENCES [Notification].[EntryOrExitPoint]([Id]),
	[RowVersion] [timestamp] NOT NULL
);
GO

CREATE TABLE [ImportNotification].[StateOfImport](
	[Id] [uniqueidentifier] NOT NULL CONSTRAINT PK_ImportNotificationStateOfImport PRIMARY KEY,
	[TransportRouteId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfImport_TransportRoute
		FOREIGN KEY REFERENCES [ImportNotification].[TransportRoute]([Id]),
	[CompetentAuthorityId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfImport_CompetentAuthority
		FOREIGN KEY REFERENCES [Lookup].[CompetentAuthority]([Id]),
	[EntryPointId] [uniqueidentifier] NOT NULL CONSTRAINT FK_ImportNotificationStateOfImport_EntryPoint
		FOREIGN KEY REFERENCES [Notification].[EntryOrExitPoint]([Id]),
	[RowVersion] [timestamp] NOT NULL
);
GO