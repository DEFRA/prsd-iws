ALTER DATABASE CURRENT 
ADD FILEGROUP FileStore;
GO

DECLARE @data_path VARCHAR(256);
DECLARE @filegroup_name VARCHAR(256);
DECLARE @sql VARCHAR(MAX);

SET @filegroup_name = DB_NAME() + '.FileStore'

SELECT @data_path = LEFT(physical_name, LEN(physical_name) - CHARINDEX('\', REVERSE(physical_name), 1) + 1)
FROM sys.database_files
WHERE name = DB_NAME()

SET @sql = 'ALTER DATABASE CURRENT
ADD FILE 
(
    NAME = ''' + @filegroup_name + ''',
    FILENAME = ''' + @data_path + @filegroup_name + '.ndf' + ''',
    SIZE = 100MB,
    FILEGROWTH = 100MB
)
TO FILEGROUP FileStore;'

EXEC(@sql)

GO