CREATE TABLE [Lookup].[IntraCountryExportAllowed](
	[ExportCompetentAuthority] INT NOT NULL,
	[ImportCompetentAuthorityID] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY CLUSTERED ([ExportCompetentAuthority], [ImportCompetentAuthorityID])
) ON [PRIMARY]
GO

ALTER TABLE [Lookup].[IntraCountryExportAllowed] WITH CHECK ADD CONSTRAINT [FK_IntraCountryExportAllowed_ExportCA] FOREIGN KEY ([ExportCompetentAuthority]) REFERENCES [Lookup].[UnitedKingdomCompetentAuthority] ([Id]);
ALTER TABLE [Lookup].[IntraCountryExportAllowed] WITH CHECK ADD CONSTRAINT [FK_IntraCountryExportAllowed_ImportCA] FOREIGN KEY ([ImportCompetentAuthorityID]) REFERENCES [Lookup].[CompetentAuthority] ([Id]);
