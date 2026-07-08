IF OBJECT_ID('[Notification].[uspGetExportWorklist]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspGetExportWorklist] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Notification].[uspGetExportWorklist]
    @CompetentAuthority INT,
    @NotificationNumber NVARCHAR(50) = NULL,
    @Officer NVARCHAR(256) = NULL,
    @Statuses NVARCHAR(MAX) = NULL, -- Comma-separated list of status IDs
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @StatusTable TABLE (StatusId INT);
    
    IF @Statuses IS NOT NULL AND LEN(@Statuses) > 0
    BEGIN
        INSERT INTO @StatusTable (StatusId)
        SELECT CAST(value AS INT)
        FROM STRING_SPLIT(@Statuses, ',');
    END

    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;

    ;WITH WorklistData AS
    (
        SELECT
            N.Id AS NotificationId,
            N.NotificationNumber,
            E.Name AS Notifier,
            ND.NameOfOfficer AS Officer,
            ND.CommencementDate AS DatePickedUpByOfficer,
            ND.TransmittedDate,
            ND.AcknowledgedDate,
            ND.ConsentedDate,
            ND.DecisionRequiredByDate,
            NA.Status,
            (SELECT TOP 1 DateAdded 
             FROM [Notification].[Audit] A 
             WHERE A.NotificationId = N.Id 
             ORDER BY A.DateAdded DESC) AS LastActionDate,
            (SELECT TOP 1 CAST(A.Type AS NVARCHAR(50))
             FROM [Notification].[Audit] A 
             WHERE A.NotificationId = N.Id 
             ORDER BY A.DateAdded DESC) AS LastActionType,
            (SELECT TOP 1 DateAdded 
             FROM [Notification].[Comments] C 
             WHERE C.NotificationId = N.Id 
             ORDER BY C.DateAdded DESC) AS LastCommentDate
        FROM
            [Notification].[Notification] N
            INNER JOIN [Notification].[NotificationAssessment] NA 
                ON N.Id = NA.NotificationApplicationId
            INNER JOIN [Notification].[NotificationDates] ND 
                ON NA.Id = ND.NotificationAssessmentId
            LEFT JOIN [Notification].[Exporter] E 
                ON N.Id = E.NotificationId
        WHERE
            N.CompetentAuthority = @CompetentAuthority
            AND (@NotificationNumber IS NULL OR N.NotificationNumber LIKE '%' + @NotificationNumber + '%')
            AND (@Officer IS NULL OR ND.NameOfOfficer LIKE '%' + @Officer + '%')
            AND (NOT EXISTS(SELECT 1 FROM @StatusTable) OR NA.Status IN (SELECT StatusId FROM @StatusTable))
    )
    SELECT 
        NotificationId,
        NotificationNumber,
        Notifier,
        Officer,
        DatePickedUpByOfficer,
        TransmittedDate,
        AcknowledgedDate,
        ConsentedDate,
        DecisionRequiredByDate,
        Status,
        LastActionDate,
        LastActionType,
        LastCommentDate,
        (SELECT COUNT(*) FROM WorklistData) AS TotalCount
    FROM WorklistData
    ORDER BY 
        CASE WHEN DecisionRequiredByDate IS NULL THEN 1 ELSE 0 END,
        DecisionRequiredByDate ASC,
        NotificationNumber ASC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO