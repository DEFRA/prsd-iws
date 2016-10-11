PRINT 'Add Region column to CompetentAuthority table.'

ALTER TABLE [Lookup].[CompetentAuthority]
ADD Region NVARCHAR(2048) NULL;
GO

PRINT 'Add OrdinalPosition column to TransitState table.'


ALTER TABLE [Notification].[TransitState]
ADD OrdinalPosition INT NULL
GO

IF (SELECT COUNT(1) FROM [Notification].[TransitState]) > 0
BEGIN
	WITH Ordinals (Id, Row) AS  (
	select TS.Id, ROW_NUMBER() OVER (PARTITION BY TS.NotificationId ORDER BY AL.EventDate ASC)
	from [Notification].TransitState as TS
	INNER JOIN [Auditing].AuditLog as AL
	ON AL.RecordId = TS.Id
	WHERE AL.EventType = 0
	AND AL.TableName = '[Notification].[TransitState]')

	UPDATE  T
	SET T.[OrdinalPosition] = O.Row
	FROM [Notification].[TransitState] AS T
	INNER JOIN Ordinals AS O
	ON O.Id = T.Id

END
GO	

ALTER TABLE [Notification].[TransitState]
ALTER COLUMN [OrdinalPosition] INT NOT NULL
GO

