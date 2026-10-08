/****** Object:  Table [Lookup].[PricingTypeSettings]    Script Date: 20/06/2025 16:29:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Lookup].[PricingTypeSettings]') AND type in (N'U'))
DROP TABLE [Lookup].[PricingTypeSettings]
GO

/****** Object:  Table [Lookup].[PricingTypeSettings]    Script Date: 20/06/2025 16:29:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Lookup].[PriceType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_PriceType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


INSERT INTO [Lookup].[PriceType] ([Description]) VALUES ('EA Fixed Additional Charge for each data change');
INSERT INTO [Lookup].[PriceType] ([Description]) VALUES ('EA Import Disposal/Recovery custom per 100 (or part of) shipments over 1000 additional charge');
INSERT INTO [Lookup].[PriceType] ([Description]) VALUES ('EA Export Recovery custom per 100 (or part of) shipments over 1000 additional charge');
INSERT INTO [Lookup].[PriceType] ([Description]) VALUES ('EA Export Disposal custom per 100 (or part of) shipments over 1000 additional charge');
INSERT INTO [Lookup].[PriceType] ([Description]) VALUES ('SEPA Fixed Additional Charge for each data change');
INSERT INTO [Lookup].[PriceType] ([Description]) VALUES ('SEPA Additional Charge per shipment for not self entering data');

GO

/****** Object:  Table [Lookup].[SystemSettings]    Script Date: 20/06/2025 16:42:19 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Lookup].[SystemSettings]') AND type in (N'U'))
DROP TABLE [Lookup].[SystemSettings]
GO

/****** Object:  Table [Lookup].[SystemSettings]    Script Date: 20/06/2025 16:42:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Lookup].[SystemSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompetentAuthority] [int] NOT NULL,
	[PriceType] [int] NOT NULL,
	[ValidFrom] [date] NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Lookup].[SystemSettings]  WITH CHECK ADD  CONSTRAINT [FK_SystemSettings_PriceType] FOREIGN KEY([PriceType])
REFERENCES [Lookup].[PriceType] ([Id])
GO

ALTER TABLE [Lookup].[SystemSettings] CHECK CONSTRAINT [FK_SystemSettings_PriceType]
GO

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (1, 1, '2024-04-01', 82);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (1, 2, '2024-04-01', 1207);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (1, 3, '2024-04-01', 1196);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (1, 4, '2024-04-01', 1338);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (2, 5, '2024-04-01', 190);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (2, 6, '2024-04-01', 27);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (2, 5, '2025-04-01', 196);

INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
VALUES (2, 6, '2025-04-01', 28);