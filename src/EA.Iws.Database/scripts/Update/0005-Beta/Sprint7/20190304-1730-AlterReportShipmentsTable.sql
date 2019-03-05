-- Add new columns
ALTER TABLE [Reports].[ShipmentsCache]
ADD [SiteOfExportName] NVARCHAR(6000) NULL,
[RejectedShipmentDate] DATE NULL