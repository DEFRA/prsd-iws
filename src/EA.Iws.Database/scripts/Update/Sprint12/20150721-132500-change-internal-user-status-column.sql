PRINT 'Renaming user approval column';

EXEC sp_RENAME '[Identity].[AspNetUsers].[IsApproved]', 'InternalUserStatus', 'COLUMN';
GO

PRINT 'Dropping approval column default constraint';
GO

DECLARE @tableName nvarchar(256) = 'AspNetUsers';
DECLARE @columnName nvarchar(256) = 'InternalUserStatus';
DECLARE @Command  nvarchar(1000);

select	@Command = 'ALTER TABLE [Identity].[AspNetUsers] drop constraint ' + D.name
from	sys.tables AS T   

JOIN    sys.default_constraints AS D       
ON		D.parent_object_id = T.object_id  

JOIN    sys.columns AS C      
ON		C.object_id = T.object_id   
AND		C.column_id = D.parent_column_id

where	T.name = @tableName
and		C.name = @columnName;

execute (@Command);

GO

PRINT 'Changing user approval column to be an int column';

ALTER TABLE [Identity].[AspNetUsers] 
ALTER COLUMN [InternalUserStatus] INT NULL;
GO

PRINT 'Updating existing records';

UPDATE [Identity].[AspNetUsers]
SET [InternalUserStatus] = NULL
WHERE IsAdmin = 0;
GO

INSERT INTO [Identity].[AspNetUserClaims]
(	
	[UserId],
    [ClaimType],
    [ClaimValue]
)
SELECT	Id,
		'internal_user_status',
		'Pending'
FROM [Identity].[AspNetUsers] AS AU
WHERE 
(
	SELECT	COUNT(1) 
	FROM	[Identity].[AspNetUserClaims] AS AUC
	WHERE	AUC.UserId = AU.Id
	AND		AUC.ClaimType = 'internal_user_status'
) = 0
AND AU.IsAdmin = 1;
GO
