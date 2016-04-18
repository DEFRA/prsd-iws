PRINT 'Rename IsAdmin column';
GO

EXEC sp_RENAME '[Identity].[AspNetUsers].[IsAdmin]', 'IsInternal', 'COLUMN';
GO

PRINT 'Dropping internal user column default constraint';
GO

DECLARE @tableName nvarchar(256) = 'AspNetUsers';
DECLARE @columnName nvarchar(256) = 'IsInternal';
DECLARE @Command  nvarchar(1000);

SELECT	@Command = 'ALTER TABLE [Identity].[AspNetUsers] drop constraint ' + D.name
FROM	sys.tables AS T   

JOIN    sys.default_constraints AS D       
ON		D.parent_object_id = T.object_id  

JOIN    sys.columns AS C      
ON		C.object_id = T.object_id   
AND		C.column_id = D.parent_column_id

WHERE	T.name = @tableName
AND		C.name = @columnName;

EXECUTE (@Command);

GO

PRINT 'Adding named default constraint';
GO

ALTER TABLE [Identity].[AspNetUsers]
ADD CONSTRAINT DF_AspNetUsers_IsInternal DEFAULT 0 FOR [IsInternal];
GO