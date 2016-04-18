CREATE SCHEMA [Draft]
    AUTHORIZATION [dbo];
GO

CREATE TABLE [Draft].[Import] (
    [Id]                   INT IDENTITY (1,1) NOT NULL,
    [ImportNotificationId] UNIQUEIDENTIFIER   NOT NULL,
    [Type]                 VARCHAR(MAX)       NOT NULL,
    [Value]                VARCHAR(MAX)       NOT NULL,
    CONSTRAINT [PK_Draft_Import] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Draft_Import_ImportNotifcation] FOREIGN KEY ([ImportNotificationId]) REFERENCES [Notification].[ImportNotification]([Id])
);

GO