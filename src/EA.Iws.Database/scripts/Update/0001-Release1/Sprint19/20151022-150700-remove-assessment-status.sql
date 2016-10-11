DECLARE @numberChanges TABLE (FormerIdValue INT, NewIdValue INT)
INSERT INTO @numberChanges
VALUES (8, 7),
(9, 8),
(10, 9),
(11, 10),
(12, 11)

DECLARE @acknowledged TABLE (Id UNIQUEIDENTIFIER)
INSERT INTO @acknowledged
SELECT Id
FROM [Notification].[NotificationAssessment]
WHERE Status = 7

UPDATE [Notification].[NotificationAssessment]
SET Status = (SELECT NewIdValue FROM @numberChanges WHERE FormerIdValue = Status)
WHERE Status IN (SELECT FormerIdValue FROM @numberChanges)

DELETE FROM [Notification].[NotificationStatusChange]
WHERE Status = 7

UPDATE [Notification].[NotificationStatusChange]
SET Status = (SELECT NewIdValue FROM @numberChanges WHERE FormerIdValue = Status)
WHERE Status IN (SELECT FormerIdValue FROM @numberChanges)


UPDATE [Lookup].[NotificationStatus]
SET Description = 'Decision required'
WHERE Id = 7

UPDATE [Lookup].[NotificationStatus]
SET Description = 'Withdrawn'
WHERE Id = 8

UPDATE [Lookup].[NotificationStatus]
SET Description = 'Objected'
WHERE Id = 9

UPDATE [Lookup].[NotificationStatus]
SET Description = 'Consented'
WHERE Id = 10

UPDATE [Lookup].[NotificationStatus]
SET Description = 'Consent withdrawn'
WHERE Id = 11

DELETE FROM [Lookup].[NotificationStatus] WHERE Id = 12

ALTER TABLE [Notification].[NotificationDates]
DROP COLUMN [DecisionDate]