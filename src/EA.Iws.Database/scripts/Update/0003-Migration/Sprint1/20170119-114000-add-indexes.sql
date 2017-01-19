CREATE INDEX [IX_Movement_NotificationId_2] ON [Notification].[Movement] ([NotificationId]) INCLUDE ([Id], [Number], [Date], [Status], [PrenotificationDate]);

CREATE INDEX [IX_ImportMovement_NotificationId_2] ON [ImportNotification].[Movement] ([NotificationId]) INCLUDE ([Id], [Number], [ActualShipmentDate], [IsCancelled], [PrenotificationDate]);

CREATE INDEX [IX_ImportNotificationStatusChange_NotificationAssessmentId] ON [ImportNotification].[NotificationStatusChange] ([NotificationAssessmentId]);

CREATE INDEX [IX_ImportNotification_CompetentAuthority] ON [ImportNotification].[Notification] ([CompetentAuthority]) INCLUDE ([Id], [NotificationNumber]);