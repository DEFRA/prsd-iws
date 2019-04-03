CREATE TABLE [Reports].[ProducerCache](
[NotificationNumber] nvarchar(50) NOT NULL,
[CompetentAuthorityId] int NOT NULL,
[NotifierName] nvarchar(3000)NOT NULL,
[ProducerName] nvarchar(3000)NOT NULL,
[ProducerAddress1] nvarchar(1024)NOT NULL,
[ProducerAddress2] nvarchar(1024)NULL,
[ProducerTownOrCity] nvarchar(1024)NOT NULL,
[ProducerPostCode] nvarchar(64)NULL,
[SiteOfExport] nvarchar(3065)NOT NULL,
[LocalArea] nvarchar(1024)NULL,
[WasteType] nvarchar(64)NOT NULL,
[NotificationStatus] nvarchar(64)NOT NULL,
[ConsigneeName] nvarchar(3000)NOT NULL,
[ConsentFrom] date NULL,
[ConsentTo] date NULL,
[NotificationReceivedDate] date NULL,
[MovementReceivedDate] date NULL,
[MovementCompletedDate] date NULL,
[EwcCode] nvarchar(max) NULL,
[YCode] nvarchar(max) NULL,
[PointOfExit] nvarchar(2048)NOT NULL,
[PointOfEntry] nvarchar(2048)NOT NULL,
[ExportCountryName] nvarchar(2048)NOT NULL,
[ImportCountryName] nvarchar(2048)NOT NULL,
[SiteOfExportName] nvarchar(3000)NULL,
[FacilityName] nvarchar(max) NULL
);


GO

CREATE NONCLUSTERED INDEX [IX_ProducerCache_CompetentAuthorityId] ON [Reports].[ProducerCache]
(
	[CompetentAuthorityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

GO


CREATE NONCLUSTERED INDEX [IX_ProducerCache_ConsentFrom] ON [Reports].[ProducerCache]
(
	[ConsentFrom] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

GO

CREATE NONCLUSTERED INDEX [IX_ProducerCache_ConsentTo] ON [Reports].[ProducerCache]
(
	[ConsentTo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

GO

CREATE NONCLUSTERED INDEX [IX_ProducerCache_NotificationReceivedDate] ON [Reports].[ProducerCache]
(
	[NotificationReceivedDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)

GO

