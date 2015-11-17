ALTER TABLE [Notification].[ImportNotification]
ADD [CompetentAuthority] INT NULL 
	CONSTRAINT FK_ImportNotification_UKCompetentAuthority FOREIGN KEY REFERENCES [Lookup].[UnitedKingdomCompetentAuthority]([Id]);
GO

UPDATE [Notification].[ImportNotification]
SET [CompetentAuthority] = 1;
GO

ALTER TABLE [Notification].[ImportNotification]
ALTER COLUMN [CompetentAuthority] INT NOT NULL;
GO