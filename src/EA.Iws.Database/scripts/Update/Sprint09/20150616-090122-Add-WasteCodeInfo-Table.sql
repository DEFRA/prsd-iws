CREATE TABLE [Business].[WasteCodeInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[WasteCodeId] [uniqueidentifier] NOT NULL,
	[WasteTypeId] [uniqueidentifier] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_WasteCodeInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Business].[WasteCodeInfo]  WITH CHECK ADD  CONSTRAINT [FK_WasteCodeInfo_WasteCode] FOREIGN KEY([WasteCodeId])
REFERENCES [Lookup].[WasteCode] ([Id])
GO

ALTER TABLE [Business].[WasteCodeInfo] CHECK CONSTRAINT [FK_WasteCodeInfo_WasteCode]
GO

ALTER TABLE [Business].[WasteCodeInfo]  WITH CHECK ADD  CONSTRAINT [FK_WasteCodeInfo_WasteType] FOREIGN KEY([WasteTypeId])
REFERENCES [Business].[WasteType] ([Id])
GO

ALTER TABLE [Business].[WasteCodeInfo] CHECK CONSTRAINT [FK_WasteCodeInfo_WasteType]
GO

ALTER TABLE [Business].[WasteType] DROP CONSTRAINT [FK_WasteType_WasteCode]
GO

ALTER TABLE [Business].[WasteType] DROP COLUMN [WasteCodeId]
GO

