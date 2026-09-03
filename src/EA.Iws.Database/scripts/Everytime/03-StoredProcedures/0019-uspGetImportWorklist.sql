IF OBJECT_ID('[ImportNotification].[uspGetImportWorklist]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspGetImportWorklist] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [ImportNotification].[uspGetImportWorklist]
    @CompetentAuthority INT,
    @NotificationNumber NVARCHAR(50) = NULL,
    @Officer NVARCHAR(256) = NULL,
    @Statuses NVARCHAR(MAX) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 20
AS
BEGIN
    SET NOCOUNT ON;

    -- Parse statuses using XML (compatible with all SQL Server versions)
    DECLARE @StatusTable TABLE (StatusId INT);
    
    IF @Statuses IS NOT NULL AND LEN(@Statuses) > 0
    BEGIN
        DECLARE @StatusesXml XML;
        SET @StatusesXml = CAST('<i>' + REPLACE(@Statuses, ',', '</i><i>') + '</i>' AS XML);
        
        INSERT INTO @StatusTable (StatusId)
        SELECT CAST(T.c.value('.', 'INT') AS INT)
        FROM @StatusesXml.nodes('/i') T(c)
        WHERE LTRIM(RTRIM(T.c.value('.', 'NVARCHAR(10)'))) <> '';
    END

    DECLARE @Offset INT;
    SET @Offset = (@PageNumber - 1) * @PageSize;

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
            FG.Status AS FinancialGuaranteeStatus,
            FGLS.Description AS FinancialGuaranteeStatusDescription,
            
             -- Last action from status change history
            (SELECT TOP 1 LS.Description
             FROM [ImportNotification].[NotificationStatusChange] NSC
             INNER JOIN [Lookup].[ImportNotificationStatus] LS ON NSC.NewStatus = LS.Id
             WHERE NSC.NotificationAssessmentId = NA.Id
             ORDER BY NSC.ChangeDate DESC) AS LastAction,
            
            -- Latest comment information
            (SELECT TOP 1 C.Comment 
             FROM [ImportNotification].[Comments] C 
             WHERE C.NotificationId = N.Id 
             ORDER BY C.DateAdded DESC) AS LastComment,
             
            (SELECT TOP 1 C.DateAdded 
             FROM [ImportNotification].[Comments] C 
             WHERE C.NotificationId = N.Id 
             ORDER BY C.DateAdded DESC) AS LastCommentDate,

            -- Latest comment author
            (SELECT TOP 1 LTRIM(RTRIM(ISNULL(U.FirstName, '') + ' ' + ISNULL(U.Surname, '')))
             FROM [ImportNotification].[Comments] C
             INNER JOIN [Identity].[AspNetUsers] U ON U.Id = C.UserId
             WHERE C.NotificationId = N.Id
             ORDER BY C.DateAdded DESC) AS LastCommentUser
             
        FROM
            [ImportNotification].[Notification] N
            INNER JOIN [ImportNotification].[NotificationAssessment] NA 
                ON N.Id = NA.NotificationApplicationId
            INNER JOIN [ImportNotification].[NotificationDates] ND 
                ON NA.Id = ND.NotificationAssessmentId
                
            LEFT JOIN [ImportNotification].[Exporter] E 
                ON N.Id = E.ImportNotificationId
                
            LEFT JOIN [ImportNotification].[FinancialGuarantee] FG 
                ON N.Id = FG.ImportNotificationId
                
            LEFT JOIN [Lookup].[FinancialGuaranteeStatus] FGLS
                ON FG.Status = FGLS.Id
                
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
        FinancialGuaranteeStatus,
        FinancialGuaranteeStatusDescription,
        LastAction,
        LastComment,
        LastCommentDate,
        LastCommentUser,
        
        (SELECT COUNT(*) FROM WorklistData) AS TotalCount
    FROM WorklistData
    ORDER BY 
        CASE WHEN DecisionRequiredByDate IS NULL THEN 1 ELSE 0 END,
        DecisionRequiredByDate ASC,
        NotificationNumber ASC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END