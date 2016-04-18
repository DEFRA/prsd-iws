
INSERT INTO [Person].[Address]
           ([Id]
           ,[Address1]
           ,[Address2]
           ,[TownOrCity]
           ,[PostalCode]
           ,[Region]
           ,[Country])
     VALUES
           (N'3f5c22ba-4fde-433f-b45d-27928ef2866a'
           ,'Test1' 
           ,'Test2'
           ,'Baroda'
           ,'22345'
           ,'Test county'
           ,'United Kingdom')
GO


UPDATE [Identity].[AspNetUsers] 
SET AddressId = N'3f5c22ba-4fde-433f-b45d-27928ef2866a'
WHERE Id = N'ac795e26-1563-4833-b8f9-0529eb9e66ae'

