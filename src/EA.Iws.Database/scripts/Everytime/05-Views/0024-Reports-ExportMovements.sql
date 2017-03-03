IF OBJECT_ID('[Reports].[ExportMovements]') IS NULL
    EXEC('CREATE VIEW [Reports].[ExportMovements] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[ExportMovements]
AS

SELECT
    M.Id,
    M.Date AS ShipmentDate,
    M.FileId,
    IU_M.Id AS MovementInternalUserId,
    MR.Date AS ReceiptDate,
    IU_MR.Id AS MovementReceiptInternalUserId,
    MOR.Date AS OperationDate,
    IU_MOR.Id AS MovementOperationReceiptInternalUserId,
    N.CompetentAuthority
FROM
    [Notification].[Movement] M
    INNER JOIN [Notification].[Notification] N ON M.NotificationId = N.Id
    INNER JOIN [Notification].[NotificationAssessment] NA ON NA.NotificationApplicationId = N.Id
    INNER JOIN [Identity].[AspNetUsers] U_M ON M.CreatedBy = U_M.Id
    LEFT JOIN [Person].[InternalUser] IU_M ON U_M.Id = IU_M.UserId
    LEFT JOIN [Notification].[MovementReceipt] MR 
        INNER JOIN [Identity].[AspNetUsers] U_MR ON MR.CreatedBy = U_MR.Id
        LEFT JOIN [Person].[InternalUser] IU_MR ON U_MR.Id = IU_MR.UserId
    ON MR.MovementId = M.Id
    LEFT JOIN [Notification].[MovementOperationReceipt] MOR 
        INNER JOIN [Identity].[AspNetUsers] U_MOR ON MOR.CreatedBy = U_MOR.Id
        LEFT JOIN [Person].[InternalUser] IU_MOR ON U_MOR.Id = IU_MOR.UserId
    ON MOR.MovementId = M.Id
WHERE
    M.PrenotificationDate IS NOT NULL