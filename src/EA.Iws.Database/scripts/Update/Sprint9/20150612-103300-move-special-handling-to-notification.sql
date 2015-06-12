-----------
-- Add new columns
-----------
ALTER TABLE [Notification].[Notification]
ADD [HasSpecialHandlingRequirements] BIT NULL

ALTER TABLE [Notification].[Notification]
ADD [SpecialHandlingDetails] NVARCHAR(2048) NULL
GO

-----------
-- Migrate data
-----------
DECLARE @notificationId uniqueidentifier
DECLARE @hasSpecialHandling bit
DECLARE @specialHandling nvarchar(2048)
DECLARE @cursor CURSOR

SET @cursor = CURSOR FOR
SELECT
NotificationId,
IsSpecialHandling,
SpecialHandlingDetails
FROM [Business].[ShipmentInfo]

OPEN @cursor
FETCH NEXT
FROM @cursor INTO @notificationId, @hasSpecialHandling, @specialHandling
WHILE @@FETCH_STATUS = 0
BEGIN
UPDATE [Notification].[Notification]
SET [HasSpecialHandlingRequirements] = @hasSpecialHandling,
[SpecialHandlingDetails] = @specialHandling
WHERE Id = @notificationId
FETCH NEXT
FROM @cursor INTO @notificationId, @hasSpecialHandling, @specialHandling

END

CLOSE @cursor
DEALLOCATE @cursor

GO

-----------
-- Drop default constraint
-----------

-- Don't know the name of the default as it was automatically created, so find what it is.
declare @table_name nvarchar(256)
declare @col_name nvarchar(256)
declare @Command  nvarchar(1000)

set @table_name = N'ShipmentInfo'
set @col_name = N'IsSpecialHandling'

select @Command = 'ALTER TABLE [Business].[' + @table_name + '] drop constraint [' + d.name + ']'
 from sys.tables t   
  join    sys.default_constraints d       
   on d.parent_object_id = t.object_id  
  join    sys.columns c      
   on c.object_id = t.object_id      
    and c.column_id = d.parent_column_id
 where t.name = @table_name
  and c.name = @col_name

execute(@Command)
GO

-----------
-- Drop unused columns
-----------
ALTER TABLE [Business].[ShipmentInfo] DROP COLUMN [IsSpecialHandling]
ALTER TABLE [Business].[ShipmentInfo] DROP COLUMN [SpecialHandlingDetails]
GO