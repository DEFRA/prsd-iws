IF OBJECT_ID('[Notification].[uspGetExportWorklist]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspGetExportWorklist] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Notification].[uspGetExportWorklist]
    @CompetentAuthority INT,
    @NotificationNumber NVARCHAR(50) = NULL,
    @Officer NVARCHAR(255) = NULL,
    @Statuses NVARCHAR(MAX) = NULL,
    @PageNumber INT = 1,
    @PageSize INT = 25
AS
BEGIN
    SET NOCOUNT ON;

    -- Parse XML status list
    DECLARE @StatusIds TABLE (Id INT);
    IF @Statuses IS NOT NULL
    BEGIN
        INSERT INTO @StatusIds
        SELECT CAST(value AS INT)
        FROM STRING_SPLIT(@Statuses, ',');
    END

    ;WITH WorklistData AS
    (
        SELECT 
            N.Id AS NotificationId,
            N.NotificationNumber,
            -- Notifier (Exporter name directly from Exporter table)
            E.Name AS Notifier,
            -- Officer from NotificationDates
            NAD.NameOfOfficer AS Officer,
            -- Date picked up by officer (using CommencementDate as proxy)
            NAD.CommencementDate AS DatePickedUpByOfficer,
            -- Transmitted date
            NAD.TransmittedDate,
            -- Acknowledged date
            NAD.AcknowledgedDate,
            -- Consented date
            NAD.ConsentedDate,
            -- Decision required date
            NAD.DecisionRequiredByDate AS DecisionRequiredByDate,
            -- Status
            NA.Status,
            -- Last action from status changes
            (
                SELECT TOP 1 LNS.Description
                FROM [Notification].[NotificationStatusChange] NSC
                INNER JOIN [Lookup].[NotificationStatus] LNS ON NSC.Status = LNS.Id
                WHERE NSC.NotificationAssessmentId = NA.Id
                ORDER BY NSC.ChangeDate DESC
            ) AS LastActionType,
            -- Last action date
            (
                SELECT TOP 1 NSC.ChangeDate
                FROM [Notification].[NotificationStatusChange] NSC
                WHERE NSC.NotificationAssessmentId = NA.Id
                ORDER BY NSC.ChangeDate DESC
            ) AS LastActionDate,
            -- Last comment date
            (
                SELECT TOP 1 C.DateAdded
                FROM [Notification].[Comments] C
                WHERE C.NotificationId = N.Id
                ORDER BY C.DateAdded DESC
            ) AS LastCommentDate,
            -- Financial Guarantee Status
            (
                SELECT TOP 1 LFGS.Description
                FROM [Notification].[FinancialGuaranteeCollection] FGC
                INNER JOIN [Notification].[FinancialGuarantee] FG ON FG.FinancialGuaranteeCollectionId = FGC.Id
                INNER JOIN [Lookup].[FinancialGuaranteeStatus] LFGS ON FG.Status = LFGS.Id
                WHERE FGC.NotificationId = N.Id
                ORDER BY FG.CreatedDate DESC
            ) AS FinancialGuaranteeStatus,
            -- Last Comment
            (
                SELECT TOP 1 C.Comment
                FROM [Notification].[Comments] C
                WHERE C.NotificationId = N.Id
                ORDER BY C.DateAdded DESC
            ) AS LastComment
        FROM [Notification].[Notification] N
        INNER JOIN [Notification].[NotificationAssessment] NA ON NA.NotificationApplicationId = N.Id
        LEFT JOIN [Notification].[NotificationDates] NAD ON NAD.NotificationAssessmentId = NA.Id
        LEFT JOIN [Notification].[Exporter] E ON E.NotificationId = N.Id
        WHERE N.CompetentAuthority = @CompetentAuthority
            AND N.NotificationType = 1 -- Export notifications only
            AND (@NotificationNumber IS NULL OR N.NotificationNumber LIKE '%' + @NotificationNumber + '%')
            AND (@Officer IS NULL OR NAD.NameOfOfficer LIKE '%' + @Officer + '%')
            AND (NOT EXISTS(SELECT 1 FROM @StatusIds) OR NA.Status IN (SELECT Id FROM @StatusIds))
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
        FinancialGuaranteeStatus,
        LastComment,
        TotalCount = (SELECT COUNT(*) FROM WorklistData)
    FROM WorklistData
    ORDER BY NotificationNumber
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO