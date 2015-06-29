CREATE TABLE [Business].[WasteAdditionalInformation](
	[Id] [uniqueidentifier] NOT NULL,
	[Constituent] [nvarchar](256) NOT NULL,
	[MinConcentration] [decimal](5, 2) NOT NULL,
	[MaxConcentration] [decimal](5, 2) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
	[WasteTypeId] [uniqueidentifier] NOT NULL,
	[WasteInformationType] [int] NOT NULL,
 CONSTRAINT [PK_WasteAdditionalInformation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [Business].[WasteAdditionalInformation]  WITH CHECK ADD  CONSTRAINT [FK_WasteAdditionalInformation_WasteType] FOREIGN KEY([WasteTypeId])
REFERENCES [Business].[WasteType] ([Id])
GO

ALTER TABLE [Business].[WasteAdditionalInformation] CHECK CONSTRAINT [FK_WasteAdditionalInformation_WasteType]
GO

ALTER TABLE [Business].[WasteComposition] ADD ChemicalCompositionType int
GO

ALTER TABLE [Business].[WasteType] ADD HasAnnex bit NOT Null
GO

ALTER TABLE [Business].[WasteType] ADD OtherWasteTypeDescription nvarchar(256) NULL
GO

ALTER TABLE [Business].[WasteType] ADD  CONSTRAINT [DF_WasteType_HasAnnex]  DEFAULT ((0)) FOR [HasAnnex]
GO

ALTER TABLE [Business].[WasteType] ADD EnergyInformation nvarchar(256) NULL
GO

ALTER TABLE [Business].[WasteType] ADD WoodTypeDescription nvarchar(256) NULL
GO

ALTER TABLE [Business].[WasteType] ADD OptionalInformation nvarchar(256) NULL
GO