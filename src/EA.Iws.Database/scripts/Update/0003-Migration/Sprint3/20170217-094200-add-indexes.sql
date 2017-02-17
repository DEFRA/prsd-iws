CREATE INDEX [IX_Notification_CompetentAuthority_2] ON [Notification].[Notification] ([CompetentAuthority]) INCLUDE ([Id], [NotificationType], [NotificationNumber]);
CREATE INDEX [IX_Producer_IsSiteOfExport] ON [Notification].[Producer] ([IsSiteOfExport]) INCLUDE ([Name], [ProducerCollectionId]);
CREATE INDEX [IX_AspNetUsers_EmailConfirmed] ON [Identity].[AspNetUsers] ([EmailConfirmed]) INCLUDE ([Id], [FirstName], [Surname]);
CREATE INDEX [IX_AspNetUsers_Email] ON [Identity].[AspNetUsers] ([Email]);
CREATE INDEX [IX_MovementReceipt_CreatedBy] ON [Notification].[MovementReceipt] ([CreatedBy]) INCLUDE ([MovementId], [Date]);
CREATE INDEX [IX_MovementOperationReceipt_CreatedBy] ON [Notification].[MovementOperationReceipt] ([CreatedBy]) INCLUDE ([Date], [MovementId]);
CREATE INDEX [IX_Movement_Date_PrenotificationDate] ON [Notification].[Movement] ([Date], [PrenotificationDate]) INCLUDE ([Id], [NotificationId], [FileId], [CreatedBy]);
CREATE INDEX [IX_Movement_NotificationId_3] ON [Notification].[Movement] ([NotificationId]) INCLUDE ([Id], [Number], [Date], [RowVersion], [FileId], [Status], [PrenotificationDate], [HasNoPrenotification], [CreatedBy]);