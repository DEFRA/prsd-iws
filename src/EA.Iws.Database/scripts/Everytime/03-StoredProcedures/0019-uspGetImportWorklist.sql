IF OBJECT_ID('[ImportNotification].[uspGetImportWorklist]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspGetImportWorklist] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [ImportNotification].[uspGetImportWorklist]
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
            E.Name AS Exporter,
            ND.NameOfOfficer AS Officer,
            ND.AssessmentStartedDate AS DatePickedUpByOfficer,
            ND.NotificationReceivedDate,
            ND.AcknowledgedDate,
            ND.ConsentedDate,
            ND.DecisionRequiredByDate,
            NA.Status,
            (SELECT TOP 1 DateAdded 
             FROM [ImportNotification].[Comments] C 
             WHERE C.NotificationId = N.Id 
             ORDER BY C.DateAdded DESC) AS LastCommentDate
        FROM
            [ImportNotification].[Notification] N
            INNER JOIN [ImportNotification].[NotificationAssessment] NA 
                ON N.Id = NA.NotificationApplicationId
            INNER JOIN [ImportNotification].[NotificationDates] ND 
                ON NA.Id = ND.NotificationAssessmentId
            LEFT JOIN [ImportNotification].[Exporter] E 
                ON N.Id = E.ImportNotificationId
        WHERE
            N.CompetentAuthority = @CompetentAuthority
            AND (@NotificationNumber IS NULL OR N.NotificationNumber LIKE '%' + @NotificationNumber + '%')
            AND (@Officer IS NULL OR ND.NameOfOfficer LIKE '%' + @Officer + '%')
            AND (NOT EXISTS(SELECT 1 FROM @StatusTable) OR NA.Status IN (SELECT StatusId FROM @StatusTable))
    )
    SELECT 
        NotificationId,
        NotificationNumber,
        Exporter,
        Officer,
        DatePickedUpByOfficer,
        NotificationReceivedDate,
        AcknowledgedDate,
        ConsentedDate,
        DecisionRequiredByDate,
        Status,
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