-- Modified from http://stackoverflow.com/a/5198894

DECLARE 
    @SchemaName varchar(255),
    @TableName varchar(255),
    @ColumnName varchar(255),
    @ForeignKeyName sysname

SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE FKColumns_cursor CURSOR Fast_Forward FOR
SELECT  cu.TABLE_SCHEMA, cu.TABLE_NAME, cu.COLUMN_NAME, cu.CONSTRAINT_NAME
FROM    INFORMATION_SCHEMA.TABLE_CONSTRAINTS ic 
    INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE cu ON ic.CONSTRAINT_NAME = cu.CONSTRAINT_NAME
WHERE   ic.CONSTRAINT_TYPE = 'FOREIGN KEY'

CREATE TABLE #temp1(    
    SchemaName varchar(255),
    TableName varchar(255),
    ColumnName varchar(255),
    ForeignKeyName sysname
)

OPEN FKColumns_cursor  
FETCH NEXT FROM FKColumns_cursor INTO @SchemaName,@TableName, @ColumnName, @ForeignKeyName

WHILE @@FETCH_STATUS = 0  
BEGIN

    IF ( SELECT COUNT(*)
    FROM        sysobjects o    
        INNER JOIN sysindexes x ON x.id = o.id
        INNER JOIN  syscolumns c ON o.id = c.id 
        INNER JOIN sysindexkeys xk ON c.colid = xk.colid AND o.id = xk.id AND x.indid = xk.indid
    WHERE       o.type in ('U')
        AND xk.keyno <= x.keycnt
        AND permissions(o.id, c.name) <> 0
        AND (x.status&32) = 0
        AND o.name = @TableName
        AND c.name = @ColumnName
    ) = 0
    BEGIN
        INSERT INTO #temp1 SELECT @SchemaName, @TableName, @ColumnName, @ForeignKeyName
    END


    FETCH NEXT FROM FKColumns_cursor INTO @SchemaName,@TableName, @ColumnName, @ForeignKeyName
END  
CLOSE FKColumns_cursor  
DEALLOCATE FKColumns_cursor 

SELECT 'CREATE NONCLUSTERED INDEX IX_' + ForeignKeyName + ' ON [' + SchemaName + '].[' + TableName + ']([' + ColumnName +']);'
FROM #temp1 
ORDER BY TableName

drop table #temp1 