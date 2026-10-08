ALTER TABLE [ImportNotification].[Exporter] ADD [Type] INT NOT NULL DEFAULT 4;
ALTER TABLE [ImportNotification].[Exporter] ADD FOREIGN KEY (Type) REFERENCES [Lookup].[BusinessType](Id);
ALTER TABLE [ImportNotification].[Exporter] ADD RegistrationNumber NVARCHAR(100) NULL;

ALTER TABLE [ImportNotification].[Producer] ADD [Type] INT NOT NULL DEFAULT 4;
ALTER TABLE [ImportNotification].[Producer] ADD FOREIGN KEY (Type) REFERENCES [Lookup].[BusinessType](Id);
ALTER TABLE [ImportNotification].[Producer] ADD RegistrationNumber NVARCHAR(100) NULL;

ALTER TABLE [Reports].[ShipmentsCache] ADD [NotifierCompanyType] INT NOT NULL DEFAULT 1;
ALTER TABLE [Reports].[ShipmentsCache] ADD [ConsigneeCompanyType] INT NOT NULL DEFAULT 1;
ALTER TABLE [Reports].[ShipmentsCache] ADD [FacilityCompanyType] INT NOT NULL DEFAULT 1;
