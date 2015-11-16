ALTER TABLE [Notification].[Carrier]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NOT NULL

ALTER TABLE [Notification].[Carrier]
ALTER COLUMN [AdditionalRegistrationNumber] NVARCHAR(100) NULL

ALTER TABLE [Notification].[Exporter]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NOT NULL

ALTER TABLE [Notification].[Exporter]
ALTER COLUMN [AdditionalRegistrationNumber] NVARCHAR(100) NULL

ALTER TABLE [Notification].[Facility]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NOT NULL

ALTER TABLE [Notification].[Facility]
ALTER COLUMN [AdditionalRegistrationNumber] NVARCHAR(100) NULL

ALTER TABLE [Notification].[Importer]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NOT NULL

ALTER TABLE [Notification].[Importer]
ALTER COLUMN [AdditionalRegistrationNumber] NVARCHAR(100) NULL

ALTER TABLE [Notification].[Producer]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NOT NULL

ALTER TABLE [Notification].[Producer]
ALTER COLUMN [AdditionalRegistrationNumber] NVARCHAR(100) NULL

ALTER TABLE [Person].[AddressBookRecord]
ALTER COLUMN [RegistrationNumber] NVARCHAR(100) NOT NULL

ALTER TABLE [Person].[AddressBookRecord]
ALTER COLUMN [AdditionalRegistrationNumber] NVARCHAR(100) NULL