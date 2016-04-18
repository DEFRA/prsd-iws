GO
PRINT N'ALTER [Business].[WasteType]...';

GO
ALTER TABLE [Business].[WasteType] ADD WasteCodeId UNIQUEIDENTIFIER NULL

GO
ALTER TABLE [Business].[WasteType]  WITH CHECK ADD  CONSTRAINT [FK_WasteType_WasteCode] FOREIGN KEY([WasteCodeId])
REFERENCES [Lookup].[WasteCode] ([Id])

GO
ALTER TABLE [Business].[WasteType] CHECK CONSTRAINT [FK_WasteType_WasteCode]

GO
PRINT N'Update complete.';
