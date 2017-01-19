CREATE INDEX [IX_WasteCodeInfo_NotificationId] ON [Notification].[WasteCodeInfo] ([NotificationId]) INCLUDE ([CodeType]);

CREATE INDEX [IX_Carrier_CarrierCollectionId] ON [Notification].[Carrier] ([CarrierCollectionId]) INCLUDE ([Id]);

CREATE INDEX [IX_Notification_CompetentAuthority] ON [Notification].[Notification] ([CompetentAuthority]) INCLUDE ([Id], [NotificationNumber]);

CREATE INDEX [IX_WasteCodeInfo_NotificationId_CodeType] ON [Notification].[WasteCodeInfo] ([NotificationId], [CodeType]) INCLUDE ([WasteCodeId]);

CREATE INDEX [IX_NotificationStatusChange_NotificationAssessmentId] ON [Notification].[NotificationStatusChange] ([NotificationAssessmentId]);