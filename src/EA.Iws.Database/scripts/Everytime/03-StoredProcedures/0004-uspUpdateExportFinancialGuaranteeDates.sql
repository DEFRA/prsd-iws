IF OBJECT_ID('[Notification].[uspUpdateExportFinancialGuaranteeDates]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspUpdateExportFinancialGuaranteeDates] AS SET NOCOUNT ON;')
GO
 
ALTER PROCEDURE [Notification].[uspUpdateExportFinancialGuaranteeDates]
    @NotificationId UNIQUEIDENTIFIER,
    @ReceivedDate DATE,
    @CompletedDate DATE,
    @DecisionDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FinancialGuaranteeCollectionId UNIQUEIDENTIFIER;
    DECLARE @CurrentFinancialGuaranteeId UNIQUEIDENTIFIER;

    SELECT @FinancialGuaranteeCollectionId = [Id]
    FROM [Notification].[FinancialGuaranteeCollection]
    WHERE [NotificationId] = @NotificationId;

    SELECT TOP 1 @CurrentFinancialGuaranteeId = [Id]
    FROM [Notification].[FinancialGuarantee]
    WHERE [FinancialGuaranteeCollectionId] = @FinancialGuaranteeCollectionId
    ORDER BY [CreatedDate] DESC;

    UPDATE [Notification].[FinancialGuarantee]
       SET [ReceivedDate] = ISNULL(@ReceivedDate, [ReceivedDate])
          ,[CompletedDate] = CASE WHEN [CompletedDate] IS NULL THEN NULL ELSE ISNULL(@CompletedDate, [CompletedDate]) END
          ,[DecisionDate] = CASE WHEN [DecisionDate] IS NULL THEN NULL ELSE ISNULL(@DecisionDate, [DecisionDate]) END
     WHERE [Id] = @CurrentFinancialGuaranteeId;
END
GO