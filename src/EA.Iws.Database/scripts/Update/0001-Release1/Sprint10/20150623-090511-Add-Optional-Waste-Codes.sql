ALTER TABLE [Business].[WasteCodeInfo] ADD OptionalCode [nvarchar](120) NULL
ALTER TABLE [Business].[WasteCodeInfo] ADD OptionalDescription [nvarchar](1024) NULL
GO
INSERT INTO [Lookup].[WasteCode]
           ([Id]
           ,[Code]
           ,[Description]
           ,[CodeType])
     VALUES
           (newid()
           ,''
           ,'Optional export code'
           ,7)
GO
INSERT INTO [Lookup].[WasteCode]
           ([Id]
           ,[Code]
           ,[Description]
           ,[CodeType])
     VALUES
           (newid()
           ,''
           ,'Optional import code'
           ,8)
GO
INSERT INTO [Lookup].[WasteCode]
           ([Id]
           ,[Code]
           ,[Description]
           ,[CodeType])
     VALUES
           (newid()
           ,''
           ,'Optional other code'
           ,9)
GO
INSERT INTO [Lookup].[WasteCode]
           ([Id]
           ,[Code]
           ,[Description]
           ,[CodeType])
     VALUES
           (newid()
           ,''
           ,'Optional custom code'
           ,10)
GO