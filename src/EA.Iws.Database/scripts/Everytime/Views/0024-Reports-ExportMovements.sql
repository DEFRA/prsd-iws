IF OBJECT_ID('[Reports].[ExportMovements]') IS NULL
    EXEC('CREATE VIEW [Reports].[ExportMovements] AS SELECT 1 AS [NOTHING];')
GO

ALTER VIEW [Reports].[ExportMovements]
AS

SELECT
    COUNT(CASE WHEN MovementInternalUserId IS NULL AND FileId IS NOT NULL THEN 1 ELSE NULL END) AS FilesUploadedExternally,
    COUNT(CASE WHEN MovementInternalUserId IS NOT NULL AND FileId IS NOT NULL THEN 1 ELSE NULL END) AS FilesUploadedInternally,
    COUNT(CASE WHEN MovementInternalUserId IS NULL THEN 1 ELSE NULL END) AS MovementsCreatedExternally,
    COUNT(MovementInternalUserId) AS MovementsCreatedInternally,
    COUNT(CASE WHEN MovementReceiptInternalUserId IS NULL AND ReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedExternally,
    COUNT(CASE WHEN MovementReceiptInternalUserId IS NOT NULL AND ReceiptDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementReceiptsCreatedInternally,
    COUNT(CASE WHEN MovementOperationReceiptInternalUserId IS NULL AND OperationDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedExternally,
    COUNT(CASE WHEN MovementOperationReceiptInternalUserId IS NOT NULL AND OperationDate IS NOT NULL THEN 1 ELSE NULL END) AS MovementOperationReceiptsCreatedInternally
FROM
(
    SELECT
        M.Id,
        M.Date AS ShipmentDate,
        M.FileId,
        IU_M.Id AS MovementInternalUserId,
        MR.Date AS ReceiptDate,
        IU_MR.Id AS MovementReceiptInternalUserId,
        MOR.Date AS OperationDate,
        IU_MOR.Id AS MovementOperationReceiptInternalUserId
    FROM
        [Notification].[Movement] M
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
) DATA