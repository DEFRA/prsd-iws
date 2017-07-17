-- Add new columns
ALTER TABLE [Reports].[ShipmentsCache]
ADD [ImportOrExport] VARCHAR(6) NULL,
[BaselOecdCode] NVARCHAR(MAX) NULL,
[OperationCodes] NVARCHAR(MAX) NULL,
[YCode] NVARCHAR(MAX) NULL,
[HCode] NVARCHAR(MAX) NULL,
[UNClass] NVARCHAR(MAX) NULL

GO

-- Set default value
UPDATE [Reports].[ShipmentsCache]
SET [ImportOrExport] = '- -';

GO

-- Make column not nullable
ALTER TABLE [Reports].[ShipmentsCache]
ALTER COLUMN [ImportOrExport] VARCHAR(6) NOT NULL