GO
PRINT N'Creating [Business].[ShippingInfoPackagingType]...';

CREATE TABLE [Business].[ShippingInfoPackagingType](
	[Id] [uniqueidentifier]				NOT NULL,
	[ShippingInfoId] [uniqueidentifier] NOT NULL,
	[PackagingType] [int]				NOT NULL,
	[OtherDescription] [nvarchar](100)	NULL,
 CONSTRAINT [PK_ShippingInfoPackagingType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [Business].[ShippingInfoPackagingType]  WITH CHECK ADD  CONSTRAINT [FK_ShippingInfoPackagingType_ShipmentInfo] FOREIGN KEY([ShippingInfoId])
REFERENCES [Business].[ShipmentInfo] ([Id])
GO

ALTER TABLE [Business].[ShippingInfoPackagingType] CHECK CONSTRAINT [FK_ShippingInfoPackagingType_ShipmentInfo]
GO

GO
PRINT N'Update complete.';


GO
