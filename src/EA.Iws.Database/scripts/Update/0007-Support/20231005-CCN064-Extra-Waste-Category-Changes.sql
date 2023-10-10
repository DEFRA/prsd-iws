CREATE TABLE [Lookup].[WasteCategoryType]
(
	[Id] INT CONSTRAINT PK_WasteCategoryType PRIMARY KEY NOT NULL,
    [Name] NVARCHAR (250)  NOT NULL   
);
GO

INSERT INTO [Lookup].[WasteCategoryType] ([Id], [Name])
VALUES	(1, 'Metals'),
		(2, 'Catalysts'),
		(3, 'WEEE'),
		(4, 'Plastics'),
		(5, 'Batteries'),
		(6, 'Clinical'),
		(7, 'Pharamaceuticals'),
		(8, 'Rugs/absorbents'),
		(9, 'Oils'),
		(10, 'Solvents/dyes'),
		(11, 'Single ship'),
		(12, 'Platform/rig'),
		(13, 'Not applicable');
GO

ALTER TABLE [Notification].[WasteType] ADD WasteCategoryType INT NULL;
GO

ALTER TABLE [Notification].[WasteType]
ADD CONSTRAINT FK_WasteCategoryType
FOREIGN KEY (WasteCategoryType) REFERENCES [Lookup].[WasteCategoryType](Id)
GO

CREATE TABLE [Lookup].[WasteComponentType]
(
	[Id] INT CONSTRAINT PK_WasteComponentType PRIMARY KEY NOT NULL,
    [Name] NVARCHAR (250)  NOT NULL   
);
GO

INSERT INTO [Lookup].[WasteComponentType] ([Id], [Name])
VALUES	(1, 'Mercury'),
		(2, 'FGas/ODS'),
		(3, 'NORM');
GO

CREATE TABLE [Notification].[WasteComponentInfo](
	[Id] [uniqueidentifier]				NOT NULL,
	[RowVersion] [timestamp]			NOT NULL,
	[WasteComponentType] [int]			NOT NULL,	
	[NotificationId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_WasteComponentInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Notification].[WasteComponentInfo]  WITH CHECK ADD  CONSTRAINT [FK_NotificationWasteComponentInfo_WasteComponentType] FOREIGN KEY([WasteComponentType])
REFERENCES [Lookup].[WasteComponentType] ([Id])
GO

ALTER TABLE [Notification].[WasteComponentInfo] CHECK CONSTRAINT [FK_NotificationWasteComponentInfo_WasteComponentType]
GO

ALTER TABLE [Notification].[WasteComponentInfo]  WITH CHECK ADD  CONSTRAINT [FK_WasteComponentInfo_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id])
GO

ALTER TABLE [Notification].[WasteComponentInfo] CHECK CONSTRAINT [FK_WasteComponentInfo_Notification]
GO

ALTER TABLE [Reports].[FreedomOfInformationCache] ADD [WasteComponentTypes] NVARCHAR(100) NULL;