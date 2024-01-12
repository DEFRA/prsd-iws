PRINT N'Creating [Lookup].[SystemSettings]...';

GO
CREATE TABLE [Lookup].[SystemSettings] (
    [Id] INT NOT NULL,
	[Value] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

PRINT N'Adding entries to [Lookup].[SystemSettings]...';

INSERT INTO [Lookup].[SystemSettings] ([Id] ,[Value] ,[Description])
	VALUES 
		(1, '2024-04-01', 'New EA charge matrix ValidFrom date'),
		(2, '2024-04-01', 'New SEPA charge matrix ValidFrom date');
GO

PRINT N'Creating [Lookup].[PricingFixedFee]...';

GO
CREATE TABLE [Lookup].[PricingFixedFee] (
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [WasteComponentTypeId] INT NULL,
	[WasteCategoryTypeId] INT NULL,
    [Price] MONEY NOT NULL,
	[ValidFrom] [date] NOT NULL,
    CONSTRAINT [PK_PricingFixedFee] PRIMARY KEY CLUSTERED ([Id] ASC)
);

ALTER TABLE [Lookup].[PricingFixedFee]  WITH CHECK ADD  CONSTRAINT [PK_PricingFixedFee_WasteComponentType] FOREIGN KEY([WasteComponentTypeId])
REFERENCES [Lookup].[WasteComponentType] ([Id])
GO

ALTER TABLE [Lookup].[PricingFixedFee]  WITH CHECK ADD  CONSTRAINT [PK_PricingFixedFee_WasteCategoryType] FOREIGN KEY([WasteCategoryTypeId])
REFERENCES [Lookup].[WasteCategoryType] ([Id])
GO

PRINT N'Adding entries to [Lookup].[PricingFixedFee]...';

DECLARE @validFrom nvarchar(100);
SET @validFrom = (SELECT [Value] from [Lookup].[SystemSettings] where Id = 1);

INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom])
	VALUES 
		('43123A19-4B53-404E-8111-154DFBA70535', null, 11, 8188, @validFrom),
		('00CB5AE5-0F09-4AC0-817F-C9C13BC3E086', null, 12, 8188, @validFrom),
		('8E4D0C26-343C-4906-BCF3-47A03DEC5E5F', 1, null, 287, @validFrom),
		('B4686BFC-CFC4-45E2-A966-2E549E623F57', 2, null, 287, @validFrom),
		('D53950B4-B81E-44B5-8C40-CB09EF41406A', 3, null, 287, @validFrom),
		('45B1AB31-9D4E-4F3A-B218-C13B22F61E5A', 4, null, 287, @validFrom)

GO