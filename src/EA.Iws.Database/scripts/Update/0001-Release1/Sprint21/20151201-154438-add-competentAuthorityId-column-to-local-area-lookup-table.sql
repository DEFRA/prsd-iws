GO

ALTER TABLE [Lookup].[LocalArea] ADD
	[CompetentAuthorityId] int

GO

GO

UPDATE [Lookup].[LocalArea] SET CompetentAuthorityId = 1

UPDATE [Lookup].[LocalArea] SET CompetentAuthorityId = 4 WHERE Name = 'Natural Resources Wales'
UPDATE [Lookup].[LocalArea] SET CompetentAuthorityId = 3 WHERE Name = 'Northern Ireland Environment Agency'
UPDATE [Lookup].[LocalArea] SET CompetentAuthorityId = 2 WHERE Name = 'Scottish Environment Protection Agency'

GO

GO

ALTER TABLE [Lookup].[LocalArea] ADD
	CONSTRAINT [FK_CompetentAuthorityId_UKCompetentAuthority] FOREIGN KEY ([CompetentAuthorityId]) REFERENCES [Lookup].[UnitedKingdomCompetentAuthority] ([Id])

GO