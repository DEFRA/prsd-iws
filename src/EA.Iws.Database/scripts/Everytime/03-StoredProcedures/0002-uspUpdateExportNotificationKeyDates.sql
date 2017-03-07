IF OBJECT_ID('[Notification].[uspUpdateExportNotificationKeyDates]') IS NULL
    EXEC('CREATE PROCEDURE [Notification].[uspUpdateExportNotificationKeyDates] AS SET NOCOUNT ON;')
GO
 
ALTER PROCEDURE [Notification].[uspUpdateExportNotificationKeyDates]
    @NotificationId UNIQUEIDENTIFIER,
    @NotificationReceivedDate DATE,
    @CommencementDate DATE,
    @CompleteDate DATE,
    @TransmittedDate DATE,
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
    FROM [Notification].[NotificationAssessment]
    WHERE [NotificationApplicationId] = @NotificationId;

    UPDATE [Notification].[NotificationDates]
       SET [NotificationReceivedDate] = CASE WHEN [NotificationReceivedDate] IS NULL THEN NULL ELSE ISNULL(@NotificationReceivedDate, [NotificationReceivedDate]) END
          ,[CommencementDate] = CASE WHEN [CommencementDate] IS NULL THEN NULL ELSE ISNULL(@CommencementDate, [CommencementDate]) END
          ,[CompleteDate] = CASE WHEN [CompleteDate] IS NULL THEN NULL ELSE ISNULL(@CompleteDate, [CompleteDate]) END
          ,[TransmittedDate] = CASE WHEN [TransmittedDate] IS NULL THEN NULL ELSE ISNULL(@TransmittedDate, [TransmittedDate]) END
          ,[AcknowledgedDate] = CASE WHEN [AcknowledgedDate] IS NULL THEN NULL ELSE ISNULL(@AcknowledgedDate, [AcknowledgedDate]) END
          ,[WithdrawnDate] = CASE WHEN [WithdrawnDate] IS NULL THEN NULL ELSE ISNULL(@WithdrawnDate, [WithdrawnDate]) END
          ,[ObjectedDate] = CASE WHEN [ObjectedDate] IS NULL THEN NULL ELSE ISNULL(@ObjectedDate, [ObjectedDate]) END
          ,[ConsentedDate] = CASE WHEN [ConsentedDate] IS NULL THEN NULL ELSE ISNULL(@ConsentedDate, [ConsentedDate]) END
     WHERE [NotificationAssessmentId] = @NotificationAssessmentId;

     UPDATE [Notification].[Consent]
        SET [From] = ISNULL(@ConsentValidFromDate, [From])
           ,[To] = ISNULL(@ConsentValidToDate, [To])
      WHERE [NotificationApplicationId] = @NotificationId;
END
GO