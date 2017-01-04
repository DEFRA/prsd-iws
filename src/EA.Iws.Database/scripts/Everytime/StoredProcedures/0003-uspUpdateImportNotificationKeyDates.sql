IF OBJECT_ID('[ImportNotification].[uspUpdateImportNotificationKeyDates]') IS NULL
    EXEC('CREATE PROCEDURE [ImportNotification].[uspUpdateImportNotificationKeyDates] AS SET NOCOUNT ON;')
GO
 
ALTER PROCEDURE [ImportNotification].[uspUpdateImportNotificationKeyDates]
    @NotificationId UNIQUEIDENTIFIER,
    @NotificationReceivedDate DATE,
    @AssessmentStartedDate DATE,
    @CompleteDate DATE,
    @AcknowledgedDate DATE,
    @WithdrawnDate DATE,
    @ObjectedDate DATE,
    @ConsentedDate DATE,
    @ConsentValidFromDate DATE,
    @ConsentValidToDate DATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NotificationAssessmentId UNIQUEIDENTIFIER;

    SELECT @NotificationAssessmentId = [Id]
    FROM [ImportNotification].[NotificationAssessment]
    WHERE [NotificationApplicationId] = @NotificationId;

    UPDATE [ImportNotification].[NotificationDates]
       SET [NotificationReceivedDate] = CASE WHEN [NotificationReceivedDate] IS NULL THEN NULL ELSE ISNULL(@NotificationReceivedDate, [NotificationReceivedDate]) END
          ,[AssessmentStartedDate] = CASE WHEN [AssessmentStartedDate] IS NULL THEN NULL ELSE ISNULL(@AssessmentStartedDate, [AssessmentStartedDate]) END
          ,[NotificationCompletedDate] = CASE WHEN [NotificationCompletedDate] IS NULL THEN NULL ELSE ISNULL(@CompleteDate, [NotificationCompletedDate]) END
          ,[AcknowledgedDate] = CASE WHEN [AcknowledgedDate] IS NULL THEN NULL ELSE ISNULL(@AcknowledgedDate, [AcknowledgedDate]) END
          ,[ConsentedDate] = CASE WHEN [ConsentedDate] IS NULL THEN NULL ELSE ISNULL(@ConsentedDate, [ConsentedDate]) END
     WHERE [NotificationAssessmentId] = @NotificationAssessmentId;

     UPDATE [ImportNotification].[Objection]
        SET [Date] = ISNULL(@ObjectedDate, [Date])
      WHERE [NotificationId] = @NotificationId

     UPDATE [ImportNotification].[Consent]
        SET [From] = ISNULL(@ConsentValidFromDate, [From])
           ,[To] = ISNULL(@ConsentValidToDate, [To])
      WHERE [NotificationId] = @NotificationId;

      UPDATE [ImportNotification].[Withdrawn]
         SET [Date] = ISNULL(@WithdrawnDate, [Date])
       WHERE [NotificationId] = @NotificationId;
END
GO