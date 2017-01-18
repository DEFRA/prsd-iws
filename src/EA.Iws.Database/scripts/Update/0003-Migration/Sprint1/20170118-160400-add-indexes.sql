CREATE INDEX [IX_WasteCodeInfo_NotificationId] ON [EA.IWS].[Notification].[WasteCodeInfo] ([NotificationId]) INCLUDE ([CodeType]);

CREATE INDEX [IX_Carrier_CarrierCollectionId] ON [EA.IWS].[Notification].[Carrier] ([CarrierCollectionId]) INCLUDE ([Id]);

CREATE INDEX [IX_Notification_CompetentAuthority] ON [EA.IWS].[Notification].[Notification] ([CompetentAuthority]) INCLUDE ([Id], [NotificationNumber]);

CREATE INDEX [IX_WasteCodeInfo_NotificationId_CodeType] ON [EA.IWS].[Notification].[WasteCodeInfo] ([NotificationId], [CodeType]) INCLUDE ([WasteCodeId]);

CREATE INDEX [IX_NotificationStatusChange_NotificationAssessmentId] ON [EA.IWS].[Notification].[NotificationStatusChange] ([NotificationAssessmentId]);