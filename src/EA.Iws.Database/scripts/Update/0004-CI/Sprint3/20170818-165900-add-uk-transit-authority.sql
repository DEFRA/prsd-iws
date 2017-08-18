DECLARE @ukId UNIQUEIDENTIFIER;
SELECT @ukId = [Id] FROM [Lookup].[Country] WHERE [Name] = 'United Kingdom';

UPDATE [Lookup].[CompetentAuthority]
SET [IsTransitAuthority] = 0
WHERE [CountryId] = @ukId;

INSERT INTO [Lookup].[CompetentAuthority]
           ([Id]
           ,[Name]
           ,[Abbreviation]
           ,[IsSystemUser]
           ,[Code]
           ,[CountryId]
           ,[Region]
           ,[IsTransitAuthority])
     VALUES
           ((SELECT Cast(Cast(Newid() AS BINARY(10)) + Cast(Getdate() AS BINARY(6)) AS UNIQUEIDENTIFIER))
           ,'Environment Agency'
           ,'EA'
           ,0
           ,'GB00'
           ,@ukId
           ,null
           ,1);