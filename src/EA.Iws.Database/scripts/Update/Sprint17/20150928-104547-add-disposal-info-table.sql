CREATE TABLE [Notification].[DisposalInfo] (
    [Id]						UNIQUEIDENTIFIER NOT NULL,
    [NotificationId]			UNIQUEIDENTIFIER NOT NULL,
    [Unit]						INT              NULL,
    [Amount]					DECIMAL (12, 2)  NULL,
	[Method]				    NVARCHAR (40)   NULL,
    [RowVersion]				ROWVERSION       NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DisposalInfo_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id])
);