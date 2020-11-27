CREATE TABLE [Lookup].[IntraCountryExportAllowed](
	[ExportCompetentAuthorityID] UNIQUEIDENTIFIER NOT NULL,
	[ImportCompetentAuthorityID] UNIQUEIDENTIFIER NOT NULL,
	PRIMARY KEY CLUSTERED ([ExportCompetentAuthorityID], [ImportCompetentAuthorityID])
) ON [PRIMARY]
GO

ALTER TABLE [Lookup].[IntraCountryExportAllowed] WITH CHECK ADD CONSTRAINT [FK_IntraCountryExportAllowed_ExportCA] FOREIGN KEY ([ExportCompetentAuthorityID]) REFERENCES [Lookup].[CompetentAuthority] ([Id]);
ALTER TABLE [Lookup].[IntraCountryExportAllowed] WITH CHECK ADD CONSTRAINT [FK_IntraCountryExportAllowed_ImportCA] FOREIGN KEY ([ImportCompetentAuthorityID]) REFERENCES [Lookup].[CompetentAuthority] ([Id]);
