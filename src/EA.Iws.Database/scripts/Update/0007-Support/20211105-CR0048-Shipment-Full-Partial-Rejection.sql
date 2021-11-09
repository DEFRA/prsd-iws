ALTER TABLE [Notification].[Movement] ADD [ShipmentType] INT NOT NULL DEFAULT 1;

ALTER TABLE [Notification].[MovementRejection] ADD [RejectedQuantity] INT NULL;
ALTER TABLE [Notification].[MovementRejection] ADD [RejectedUnit] INT NULL;

CREATE TABLE [Notification].[MovementPartialRejection](
	[Id] [uniqueidentifier] NOT NULL,
	[MovementId] [uniqueidentifier] NOT NULL,
	[ActualQuantity] [decimal](18, 4) NULL,
	[ActualUnit] [int] NULL,
	[RejectedQuantity] [decimal](18, 4) NULL,
	[RejectedUnit] [int] NULL,
	[Date] [date] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[FileId] [uniqueidentifier] NULL,
	[RowVersion] [timestamp] NOT NULL,	
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
