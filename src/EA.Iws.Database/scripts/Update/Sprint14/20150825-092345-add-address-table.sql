
CREATE TABLE [Person].[Address](
	[Id] [uniqueidentifier] NOT NULL,
	[Address1] [nvarchar](1024) NOT NULL,
	[Address2] [nvarchar](1024) NULL,
	[TownOrCity] [nvarchar](1024) NOT NULL,
	[PostalCode] [nvarchar](64) NULL,
	[Region] [nvarchar](1024) NULL,
	[Country] [nvarchar](1024) NOT NULL,
	[RowVersion] [timestamp] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE [Identity].[AspNetUsers] ADD [AddressId] [uniqueidentifier] NULL
GO

DECLARE @tempAddresses TABLE 
([Id] UNIQUEIDENTIFIER, [Address1] NVARCHAR(max)
      ,[TownOrCity] NVARCHAR(max)
      ,[Address2] NVARCHAR(max)
      ,[Region] NVARCHAR(max)
      ,[PostalCode] NVARCHAR(max)
      ,[Country] NVARCHAR(max)
	  ,[OrganisationId] UNIQUEIDENTIFIER)

INSERT INTO @tempAddresses
SELECT newid()
	   ,[Address1]
      ,[TownOrCity]
      ,[Address2]
      ,[Region]
      ,[PostalCode]
      ,[Country],
	  Id FROM [Notification].[Organisation]


INSERT INTO [Person].[Address] ([Id], [Address1]
      ,[TownOrCity]
      ,[Address2]
      ,[Region]
      ,[PostalCode]
      ,[Country])
SELECT Id
	   ,[Address1]
      ,[TownOrCity]
      ,[Address2]
      ,[Region]
      ,[PostalCode]
      ,[Country] FROM @tempAddresses

UPDATE u SET u.AddressId = a.Id
FROM [Identity].[AspNetUsers] u
INNER JOIN @tempAddresses a ON u.OrganisationId = a.OrganisationId

GO

ALTER TABLE [Notification].[Organisation] DROP COLUMN [Address1] 
ALTER TABLE [Notification].[Organisation] DROP COLUMN [TownOrCity] 
ALTER TABLE [Notification].[Organisation] DROP COLUMN [Address2] 
ALTER TABLE [Notification].[Organisation] DROP COLUMN [Region] 
ALTER TABLE [Notification].[Organisation] DROP COLUMN [PostalCode]
ALTER TABLE [Notification].[Organisation] DROP COLUMN [Country]

GO

