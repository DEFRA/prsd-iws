INSERT INTO [Lookup].[MovementStatus] ([Id], [Status]) VALUES(8, 'PartiallyRejected')
INSERT INTO [Lookup].[MovementAuditType] ([Id], [AuditType]) VALUES(11, 'PartiallyRejected')

ALTER TABLE [Notification].[MovementRejection] ADD [RejectedQuantity] decimal(18,4) NULL;
ALTER TABLE [Notification].[MovementRejection] ADD [RejectedUnit] INT NULL;
GO

CREATE TABLE [Notification].[MovementPartialRejection](
	[Id] [uniqueidentifier] NOT NULL,
	[MovementId] [uniqueidentifier] NOT NULL,
	[ActualQuantity] [decimal](18, 4) NULL,
	[ActualUnit] [int] NULL,
	[RejectedQuantity] [decimal](18, 4) NULL,
	[RejectedUnit] [int] NULL,
	[WasteReceivedDate] [date] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[FileId] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[WasteDisposedDate] [date] NULL,
 CONSTRAINT [PK_MovementPartialRejection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Notification].[MovementPartialRejection]  WITH CHECK ADD  CONSTRAINT [FK_MovementPartialRejection_Movement] FOREIGN KEY([MovementId])
REFERENCES [Notification].[Movement] ([Id])
GO

ALTER TABLE [Notification].[MovementPartialRejection] CHECK CONSTRAINT [FK_MovementPartialRejection_Movement]
GO

ALTER TABLE [ImportNotification].[MovementRejection] ADD [RejectedQuantity] decimal(18,4) NOT NULL;
ALTER TABLE [ImportNotification].[MovementRejection] ADD [RejectedUnit] INT NOT NULL;
GO

CREATE TABLE [ImportNotification].[MovementPartialRejection](
	[Id] [uniqueidentifier] NOT NULL,
	[MovementId] [uniqueidentifier] NOT NULL,
	[ActualQuantity] [decimal](18, 4) NULL,
	[ActualUnit] [int] NULL,
	[RejectedQuantity] [decimal](18, 4) NULL,
	[RejectedUnit] [int] NULL,
	[WasteReceivedDate] [date] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[FileId] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,
	[WasteDisposedDate] [date] NULL,
 CONSTRAINT [PK_MovementPartialRejection] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO