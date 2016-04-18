GO
PRINT N'Creating [Business].[PackagingInfo]...';


CREATE TABLE [Business].[PackagingInfo](
	[Id] [uniqueidentifier]				NOT NULL,
	[ShipmentInfoId] [uniqueidentifier] NOT NULL,
	[PackagingType] [int]				NOT NULL,
	[RowVersion] [timestamp]			NOT NULL,
	[OtherDescription] [nvarchar](100)  NULL,
 CONSTRAINT [PK_PackagingInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Business].[PackagingInfo]  WITH CHECK ADD  CONSTRAINT [FK_PackagingInfo_ShipmentInfo] FOREIGN KEY([ShipmentInfoId])
REFERENCES [Business].[ShipmentInfo] ([Id])
GO

ALTER TABLE [Business].[PackagingInfo] CHECK CONSTRAINT [FK_PackagingInfo_ShipmentInfo]
GO




GO
PRINT N'Update complete.';


GO
