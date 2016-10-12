DECLARE @wasteCodes table(
NotificationId uniqueidentifier,
WasteCodes nvarchar(max)
)

DECLARE @customscodeid uniqueidentifier
SET @customscodeid =
( 
    SELECT id FROM [Lookup].[WasteCode] WHERE [CodeType] = 10
)

INSERT INTO @wasteCodes
SELECT DISTINCT WCI2.NotificationId, STUFF(
             (SELECT ', ' + WCI.[CustomCode] 
              FROM [Business].[WasteCodeInfo] WCI
              INNER JOIN [Lookup].[WasteCode] WC ON WC.Id = WCI.WasteCodeId
              WHERE WCI.[CodeType] = 10
              AND WCI.NotificationId = WCI2.NotificationId
              FOR XML PATH (''))
             , 1, 1, '')
FROM [Business].[WasteCodeInfo] WCI2 where WCI2.[WasteCodeId] = @customscodeid

DELETE FROM [Business].[WasteCodeInfo] WHERE ID NOT IN
(
    SELECT MIN(Id) AS Id FROM [Business].[WasteCodeInfo]
    GROUP BY [WasteCodeId], [NotificationId]
    HAVING [WasteCodeId] = @customscodeid
)
AND [WasteCodeId] = @customscodeid

UPDATE WCI
SET CustomCode = T.WasteCodes 
FROM [Business].[WasteCodeInfo] WCI
INNER JOIN @wasteCodes T ON WCI.NotificationId = T.NotificationId
INNER JOIN [Lookup].WasteCode WC ON WCI.WasteCodeId = WC.Id
WHERE WC.Id = @customscodeid