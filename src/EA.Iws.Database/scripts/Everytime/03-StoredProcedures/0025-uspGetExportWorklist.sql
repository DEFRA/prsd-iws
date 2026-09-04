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

    -- Normalise parameters
    SET @NotificationNumber = NULLIF(LTRIM(RTRIM(@NotificationNumber)), '');
    SET @Officer = NULLIF(LTRIM(RTRIM(@Officer)), '');
    SET @Statuses = NULLIF(LTRIM(RTRIM(@Statuses)), '');

    IF @PageNumber < 1
        SET @PageNumber = 1;

    IF @PageSize < 1
        SET @PageSize = 25;

    -- Parse status IDs
    DECLARE @StatusIds TABLE
    (
        Id INT PRIMARY KEY
    );

    DECLARE @HasStatusFilter BIT = 0;

    IF @Statuses IS NOT NULL
    BEGIN
        DECLARE @xml XML;

        SET @xml = CAST(
            '<i>' +
            REPLACE(@Statuses, ',', '</i><i>') +
            '</i>'
            AS XML
        );

        INSERT INTO @StatusIds (Id)
        SELECT DISTINCT TRY_CAST(x.i.value('.', 'NVARCHAR(100)') AS INT)
        FROM @xml.nodes('/i') AS x(i)
        WHERE TRY_CAST(x.i.value('.', 'NVARCHAR(100)') AS INT) IS NOT NULL;

        IF EXISTS (SELECT 1 FROM @StatusIds)
            SET @HasStatusFilter = 1;
    END;

    -- Main query
    SELECT
        N.Id AS NotificationId,
        N.NotificationNumber,
        E.Name AS Notifier,
        NAD.NameOfOfficer AS Officer,
        NAD.CommencementDate AS DatePickedUpByOfficer,
        NAD.TransmittedDate,
        NAD.AcknowledgedDate,
        NAD.ConsentedDate,
        NAD.DecisionRequiredByDate,
        NA.Status,

        LastStatus.ChangeDate AS LastActionDate,
        LastStatus.Description AS LastActionType,

        LastComment.DateAdded AS LastCommentDate,
        FinancialGuarantee.StatusDescription AS FinancialGuaranteeStatus,
        LastComment.UserName AS LastCommentUser,

        COUNT(*) OVER() AS TotalCount

    FROM [Notification].[Notification] N

    INNER JOIN [Notification].[NotificationAssessment] NA
        ON NA.NotificationApplicationId = N.Id

    LEFT JOIN [Notification].[NotificationDates] NAD
        ON NAD.NotificationAssessmentId = NA.Id

    LEFT JOIN [Notification].[Exporter] E
        ON E.NotificationId = N.Id

    -- Latest notification status change
    OUTER APPLY
    (
        SELECT TOP (1)
            NSC.ChangeDate,
            LNS.Description
        FROM [Notification].[NotificationStatusChange] NSC
        INNER JOIN [Lookup].[NotificationStatus] LNS
            ON LNS.Id = NSC.Status
        WHERE NSC.NotificationAssessmentId = NA.Id
        ORDER BY NSC.ChangeDate DESC
    ) LastStatus

    -- Latest comment
    OUTER APPLY
    (
        SELECT TOP (1)
            C.DateAdded,
            LTRIM(RTRIM(
                ISNULL(U.FirstName, '') + ' ' + ISNULL(U.Surname, '')
            )) AS UserName
        FROM [Notification].[Comments] C
        LEFT JOIN [Identity].[AspNetUsers] U
            ON U.Id = C.UserId
        WHERE C.NotificationId = N.Id
        ORDER BY C.DateAdded DESC
    ) LastComment

    -- Latest financial guarantee
    OUTER APPLY
    (
        SELECT TOP (1)
            LFGS.Description AS StatusDescription
        FROM [Notification].[FinancialGuaranteeCollection] FGC
        INNER JOIN [Notification].[FinancialGuarantee] FG
            ON FG.FinancialGuaranteeCollectionId = FGC.Id
        INNER JOIN [Lookup].[FinancialGuaranteeStatus] LFGS
            ON LFGS.Id = FG.Status
        WHERE FGC.NotificationId = N.Id
        ORDER BY FG.CreatedDate DESC
    ) FinancialGuarantee

    WHERE
        N.CompetentAuthority = @CompetentAuthority
        AND N.NotificationType = 1

        AND
        (
            @NotificationNumber IS NULL
            OR N.NotificationNumber LIKE '%' + @NotificationNumber + '%'
        )

        AND
        (
            @Officer IS NULL
            OR NAD.NameOfOfficer LIKE '%' + @Officer + '%'
        )

        AND
        (
            @HasStatusFilter = 0
            OR EXISTS
            (
                SELECT 1
                FROM @StatusIds S
                WHERE S.Id = NA.Status
            )
        )

    ORDER BY
        N.NotificationNumber,
        N.Id

    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY

    OPTION (RECOMPILE);
END
GO