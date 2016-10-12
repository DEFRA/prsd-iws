CREATE NONCLUSTERED INDEX IX_Producer_NotificationId
    ON [Business].[Producer] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_Facility_NotificationId
    ON [Business].[Facility] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_Exporter_NotificationId
    ON [Business].[Exporter] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_Importer_NotificationId
    ON [Business].[Importer] ([NotificationId]); 
GO


CREATE NONCLUSTERED INDEX IX_Carrier_NotificationId
    ON [Business].[Carrier] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_OperationCodes_NotificationId
    ON [Business].[OperationCodes] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_StateOfExport_NotificationId
    ON [Notification].[StateOfExport] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_StateOfImport_NotificationId
    ON [Notification].[StateOfImport] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_TransitState_NotificationId
    ON [Notification].[TransitState] ([NotificationId]); 
GO

CREATE NONCLUSTERED INDEX IX_NotificationAssessment_NotificationId
    ON [Notification].[NotificationAssessment] ([NotificationApplicationId]); 
GO

CREATE NONCLUSTERED INDEX IX_FinancialGuarantee_NotificationId
    ON [Notification].[FinancialGuarantee] ([NotificationApplicationId]); 
GO
