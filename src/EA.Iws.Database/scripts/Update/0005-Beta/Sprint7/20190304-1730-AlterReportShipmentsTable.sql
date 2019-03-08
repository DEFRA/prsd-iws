-- Add new columns
ALTER TABLE [Reports].[ShipmentsCache]
ADD [SiteOfExportName] NVARCHAR(4000) NULL,
[RejectedShipmentDate] DATE NULL