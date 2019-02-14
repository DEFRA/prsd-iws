CREATE TABLE [Notification].[EntryExitCustomsSelection](
	[Id] [uniqueidentifier] NOT NULL,
	[Entry] [bit] NULL,
	[Exit] [bit] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[TransportRouteId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_EntryExitCustomsSelection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Notification].[EntryExitCustomsSelection]  WITH CHECK ADD  CONSTRAINT [FK_EntryExitCustomsSelection_TransportRoute] FOREIGN KEY([TransportRouteId])
REFERENCES [Notification].[TransportRoute] ([Id])
GO

ALTER TABLE [Notification].[EntryExitCustomsSelection] CHECK CONSTRAINT [FK_EntryExitCustomsSelection_TransportRoute]
GO


