GO
CREATE TABLE [Business].[PhysicalCharacteristicsInfo](
	[Id] [uniqueidentifier] NOT NULL,
	[WasteTypeId] [uniqueidentifier] NOT NULL,
	[PhysicalCharacteristicType] [int] NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[OtherDescription] [nvarchar](100) NULL,
 CONSTRAINT [PK_PhysicalCharacteristicsInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Business].[PhysicalCharacteristicsInfo]  WITH CHECK ADD  CONSTRAINT [FK_PhysicalCharacteristicsInfo_WasteType] FOREIGN KEY([WasteTypeId])
REFERENCES [Business].[WasteType] ([Id])
GO

ALTER TABLE [Business].[PhysicalCharacteristicsInfo] CHECK CONSTRAINT [FK_PhysicalCharacteristicsInfo_WasteType]
GO


