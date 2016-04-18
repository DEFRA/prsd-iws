GO
PRINT N'ALTER [Business].[WasteType]...';

GO
ALTER TABLE [Business].[WasteType]
ADD WasteGenerationProcess [nvarchar](1024) NULL

GO
ALTER TABLE [Business].[WasteType] 
ADD IsDocumentAttached [bit] NOT NULL

GO
ALTER TABLE [Business].[WasteType] ADD CONSTRAINT [DF_WasteType_IsDocumentAttached]  DEFAULT ((0)) FOR [IsDocumentAttached]

GO
PRINT N'Update complete.';