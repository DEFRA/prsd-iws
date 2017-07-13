-- Add new columns
ALTER TABLE [Reports].[FreedomOfInformationCache]
ADD [BaselOecdCode] NVARCHAR(MAX) NULL;

ALTER TABLE [Reports].[FreedomOfInformationCache]
ADD [ExportCountryName] NVARCHAR(2048) NULL;

GO

-- Set default value
UPDATE [Reports].[FreedomOfInformationCache]
SET [BaselOecdCode] = '- -',
	[ExportCountryName] = '- -';

GO

-- Make columns not nullable
ALTER TABLE [Reports].[FreedomOfInformationCache]
ALTER COLUMN [BaselOecdCode] NVARCHAR(MAX) NOT NULL;

ALTER TABLE [Reports].[FreedomOfInformationCache]
ALTER COLUMN [ExportCountryName] NVARCHAR(2048) NOT NULL;

GO