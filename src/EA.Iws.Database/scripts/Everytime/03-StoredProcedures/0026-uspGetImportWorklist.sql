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

    -- Normalise parameters
    SET @NotificationNumber = NULLIF(LTRIM(RTRIM(@NotificationNumber)), '');
    SET @Officer = NULLIF(LTRIM(RTRIM(@Officer)), '');
    SET @Statuses = NULLIF(LTRIM(RTRIM(@Statuses)), '');

    IF @PageNumber < 1
        SET @PageNumber = 1;

    IF @PageSize < 1
        SET @PageSize = 20;

    -- Parse status IDs
    DECLARE @StatusTable TABLE
    (
        StatusId INT PRIMARY KEY
    );

    DECLARE @HasStatusFilter BIT = 0;

    IF @Statuses IS NOT NULL
    BEGIN
        -- Convert comma-separated values into XML, then shred.
        -- Wrap values in <i> nodes; this handles values like '1,2,3'.
        DECLARE @StatusesXml XML;

        SET @StatusesXml = CAST(
            '<i>' +
            REPLACE(@Statuses, ',', '</i><i>') +
            '</i>'
            AS XML
        );

        INSERT INTO @StatusTable (StatusId)
        SELECT DISTINCT TRY_CAST(T.c.value('.', 'NVARCHAR(10)') AS INT)
        FROM @StatusesXml.nodes('/i') T(c)
        WHERE TRY_CAST(T.c.value('.', 'NVARCHAR(10)') AS INT) IS NOT NULL;

        IF EXISTS (SELECT 1 FROM @StatusTable)
            SET @HasStatusFilter = 1;
    END;

    -- Main query
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

        LastStatus.Description AS LastAction,

        LastComment.DateAdded AS LastCommentDate,
        LastComment.UserName AS LastCommentUser,

        COUNT(*) OVER() AS TotalCount

    FROM [ImportNotification].[Notification] N

    INNER JOIN [ImportNotification].[NotificationAssessment] NA
        ON NA.NotificationApplicationId = N.Id

    INNER JOIN [ImportNotification].[NotificationDates] ND
        ON ND.NotificationAssessmentId = NA.Id

    LEFT JOIN [ImportNotification].[Exporter] E
        ON E.ImportNotificationId = N.Id

    LEFT JOIN [ImportNotification].[FinancialGuarantee] FG
        ON FG.ImportNotificationId = N.Id

    LEFT JOIN [Lookup].[FinancialGuaranteeStatus] FGLS
        ON FGLS.Id = FG.Status

    -- Latest notification status change
    OUTER APPLY
    (
        SELECT TOP (1)
            LS.Description
        FROM [ImportNotification].[NotificationStatusChange] NSC

        INNER JOIN [Lookup].[ImportNotificationStatus] LS
            ON LS.Id = NSC.NewStatus

        WHERE NSC.NotificationAssessmentId = NA.Id

        ORDER BY NSC.ChangeDate DESC
    ) LastStatus

    -- Latest comment and author
    OUTER APPLY
    (
        SELECT TOP (1)
            C.DateAdded,
            LTRIM(RTRIM(
                ISNULL(U.FirstName, '') + ' ' + ISNULL(U.Surname, '')
            )) AS UserName

        FROM [ImportNotification].[Comments] C

        LEFT JOIN [Identity].[AspNetUsers] U
            ON U.Id = C.UserId

        WHERE C.NotificationId = N.Id

        ORDER BY C.DateAdded DESC
    ) LastComment

    -- Filters
    WHERE
        N.CompetentAuthority = @CompetentAuthority

        AND
        (
            @NotificationNumber IS NULL
            OR N.NotificationNumber LIKE '%' + @NotificationNumber + '%'
        )

        AND
        (
            @Officer IS NULL
            OR ND.NameOfOfficer LIKE '%' + @Officer + '%'
        )

        AND
        (
            @HasStatusFilter = 0
            OR EXISTS
            (
                SELECT 1
                FROM @StatusTable S
                WHERE S.StatusId = NA.Status
            )
        )

    -- Pagination
    ORDER BY
        CASE WHEN ND.DecisionRequiredByDate IS NULL THEN 1 ELSE 0 END,
        ND.DecisionRequiredByDate ASC,
        N.NotificationNumber ASC,
        N.Id ASC

    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY

    OPTION (RECOMPILE);
END
GO