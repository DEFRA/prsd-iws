GO

--This script first updates existing string data (for ex- Sole Trader) to translated value as string (in this case 2 as string) and then alter column to int
UPDATE [Notification].[Carrier]
SET [Type] = CASE 
				WHEN [Type] = 'Limited Company' THEN '1'
				WHEN [Type] = 'Sole Trader' THEN '2'
				WHEN [Type] = 'Partnership' THEN '3'
				WHEN [Type] = 'Other' THEN '4'
			END
GO
ALTER TABLE [Notification].[Carrier]
ALTER COLUMN [Type] INT NOT NULL;
GO

UPDATE [Notification].[Exporter]
SET [Type] = CASE 
				WHEN [Type] = 'Limited Company' THEN '1'
				WHEN [Type] = 'Sole Trader' THEN '2'
				WHEN [Type] = 'Partnership' THEN '3'
				WHEN [Type] = 'Other' THEN '4'
			END
GO
ALTER TABLE [Notification].[Exporter]
ALTER COLUMN [Type] INT NOT NULL;
GO

UPDATE [Notification].[Facility]
SET [Type] = CASE 
				WHEN [Type] = 'Limited Company' THEN '1'
				WHEN [Type] = 'Sole Trader' THEN '2'
				WHEN [Type] = 'Partnership' THEN '3'
				WHEN [Type] = 'Other' THEN '4'
			END
GO
ALTER TABLE [Notification].[Facility]
ALTER COLUMN [Type] INT NOT NULL;
GO

UPDATE [Notification].[Importer]
SET [Type] = CASE 
				WHEN [Type] = 'Limited Company' THEN '1'
				WHEN [Type] = 'Sole Trader' THEN '2'
				WHEN [Type] = 'Partnership' THEN '3'
				WHEN [Type] = 'Other' THEN '4'
			END
GO
ALTER TABLE [Notification].[Importer]
ALTER COLUMN [Type] INT NOT NULL;
GO

UPDATE [Notification].[Organisation]
SET [Type] = CASE 
				WHEN [Type] = 'Limited Company' THEN '1'
				WHEN [Type] = 'Sole Trader' THEN '2'
				WHEN [Type] = 'Partnership' THEN '3'
				WHEN [Type] = 'Other' THEN '4'
			END
GO
ALTER TABLE [Notification].[Organisation]
ALTER COLUMN [Type] INT NOT NULL;
GO

UPDATE [Notification].[Producer]
SET [Type] = CASE 
				WHEN [Type] = 'Limited Company' THEN '1'
				WHEN [Type] = 'Sole Trader' THEN '2'
				WHEN [Type] = 'Partnership' THEN '3'
				WHEN [Type] = 'Other' THEN '4'
			END
GO
ALTER TABLE [Notification].[Producer]
ALTER COLUMN [Type] INT NOT NULL;
GO
