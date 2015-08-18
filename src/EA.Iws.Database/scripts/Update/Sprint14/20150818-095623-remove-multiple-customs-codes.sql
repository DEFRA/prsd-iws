DECLARE @wasteCodes table(
NotificationId uniqueidentifier,
WasteCodes nvarchar(max)
)

DECLARE @customscodeid uniqueidentifier
SET @customscodeid =
( 
    SELECT id FROM [Lookup].[WasteCode] WHERE codetype = 10
)

INSERT INTO @wasteCodes
SELECT DISTINCT WC2.NotificationId, STUFF(
             (SELECT ', ' + [CustomCode] 
              FROM [Business].[WasteCodeInfo] WCI
              INNER JOIN [Lookup].[WasteCode] WC ON WC.Id = WCI.WasteCodeId
              WHERE codetype = 10
              AND NotificationId = WC2.NotificationId
              FOR XML PATH (''))
             , 1, 1, '')
FROM [Business].[WasteCodeInfo] WC2 where wastecodeid = @customscodeid

DELETE FROM [Business].[WasteCodeInfo] WHERE ID NOT IN
(
    SELECT MIN(id) AS ID FROM [Business].[WasteCodeInfo]
    GROUP BY wastecodeid, notificationid
    HAVING wastecodeid = @customscodeid
)
AND wastecodeid = @customscodeid

UPDATE WCI
SET CustomCode = T.WasteCodes 
FROM [Business].[WasteCodeInfo] WCI
INNER JOIN @wasteCodes T ON WCI.NotificationId = T.NotificationId
INNER JOIN [Lookup].WasteCode WC ON WCI.WasteCodeId = WC.Id
WHERE WC.Id = @customscodeid