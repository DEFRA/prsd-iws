GO
CREATE TABLE [Notification].[TechnologyEmployed]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [AnnexProvided] bit NOT NULL, 
    [Details] NVARCHAR(MAX) NULL, 
    [NotificationId] UNIQUEIDENTIFIER NOT NULL, 
    [RowVersion] ROWVERSION NOT NULL
);

GO
ALTER TABLE [Notification].[TechnologyEmployed] WITH NOCHECK
    ADD CONSTRAINT [FK_TechnologyEmployed_Notification] FOREIGN KEY ([NotificationId]) REFERENCES [Notification].[Notification] ([Id]);


GO

GO
ALTER TABLE [Notification].[TechnologyEmployed] WITH CHECK CHECK CONSTRAINT [FK_TechnologyEmployed_Notification];


GO