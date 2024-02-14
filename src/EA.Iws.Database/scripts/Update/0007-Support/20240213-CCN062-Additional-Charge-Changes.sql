PRINT N'Creating [Notification].[AdditionalCharges] Table...';

CREATE TABLE [Notification].[AdditionalCharges](
	[Id] [uniqueidentifier] NOT NULL,
	[NotificationId] [uniqueidentifier] NOT NULL,
	[ChargeDate] [datetime] NOT NULL,
	[ChargeAmount] [money] NOT NULL,
	[ChangeDetailType] [int] NOT NULL,
	[Comments] [nvarchar](max) NULL,
	[RowVersion] [timestamp] NOT NULL
 CONSTRAINT [PK_Notification_AdditionalCharges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Notification].[AdditionalCharges]  WITH CHECK ADD  CONSTRAINT [FK_AdditionalCharges_Notification] FOREIGN KEY([NotificationId])
REFERENCES [Notification].[Notification] ([Id])
GO

ALTER TABLE [Notification].[AdditionalCharges] CHECK CONSTRAINT [FK_AdditionalCharges_Notification]
GO

