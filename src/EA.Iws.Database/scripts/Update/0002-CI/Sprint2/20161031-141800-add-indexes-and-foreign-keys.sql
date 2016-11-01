-- Missing foreign key
IF NOT EXISTS (
    SELECT 1 FROM SYS.FOREIGN_KEYS
    WHERE NAME = 'FK_ImportNotificationAssessment_NotificationApplicationId'
    AND PARENT_OBJECT_ID = OBJECT_ID('[ImportNotification].[NotificationAssessment]'))
BEGIN
    ALTER TABLE [ImportNotification].[NotificationAssessment]
    ADD CONSTRAINT FK_ImportNotificationAssessment_NotificationApplicationId
    FOREIGN KEY (NotificationApplicationId)
    REFERENCES [ImportNotification].[Notification] (Id);
END

-- Export

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_Movement_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[Movement]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Movement_NotificationId
        ON [Notification].[Movement] ([NotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_MovementReceipt_MovementId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[MovementReceipt]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_MovementReceipt_MovementId
        ON [Notification].[MovementReceipt] ([MovementId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_MovementOperationReceipt_MovementId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[MovementOperationReceipt]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_MovementOperationReceipt_MovementId
        ON [Notification].[MovementOperationReceipt] ([MovementId]);
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_MovementRejection_MovementId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[MovementRejection]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_MovementRejection_MovementId
        ON [Notification].[MovementRejection] ([MovementId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_MovementDetails_MovementId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[MovementDetails]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_MovementDetails_MovementId
        ON [Notification].[MovementDetails] ([MovementId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_Consent_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[Consent]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Consent_NotificationId
        ON [Notification].[Consent] ([NotificationApplicationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_Consultation_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[Consultation]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Consultation_NotificationId
        ON [Notification].[Consultation] ([NotificationId]);
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_Consultation_LocalAreaId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[Consultation]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Consultation_LocalAreaId
        ON [Notification].[Consultation] ([LocalAreaId]);  
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_FacilityCollection_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[FacilityCollection]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_FacilityCollection_NotificationId
        ON [Notification].[FacilityCollection] ([NotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ProducerCollection_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[ProducerCollection]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ProducerCollection_NotificationId
        ON [Notification].[ProducerCollection] ([NotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_Producer_ProducerCollectionId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[Producer]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Producer_ProducerCollectionId
        ON [Notification].[Producer] ([ProducerCollectionId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_Facility_FacilityCollectionId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[Facility]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_Facility_FacilityCollectionId
        ON [Notification].[Facility] ([FacilityCollectionId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_NotificationDates_NotificationAssessmentId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[NotificationDates]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_NotificationDates_NotificationAssessmentId
        ON [Notification].[NotificationDates] ([NotificationAssessmentId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ShipmentInfo_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[Notification].[ShipmentInfo]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ShipmentInfo_NotificationId
        ON [Notification].[ShipmentInfo] ([NotificationId]); 
END

-- Import

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportMovement_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Movement]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportMovement_NotificationId
        ON [ImportNotification].[Movement] ([NotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportMovementReceipt_MovementId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[MovementReceipt]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportMovementReceipt_MovementId
        ON [ImportNotification].[MovementReceipt] ([MovementId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportMovementOperationReceipt_MovementId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[MovementOperationReceipt]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportMovementOperationReceipt_MovementId
        ON [ImportNotification].[MovementOperationReceipt] ([MovementId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportMovementRejection_MovementId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[MovementRejection]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportMovementRejection_MovementId
        ON [ImportNotification].[MovementRejection] ([MovementId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportConsent_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Consent]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportConsent_NotificationId
        ON [ImportNotification].[Consent] ([NotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportConsultation_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Consultation]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportConsultation_NotificationId
        ON [ImportNotification].[Consultation] ([NotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportConsultation_LocalAreaId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Consultation]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportConsultation_LocalAreaId
        ON [ImportNotification].[Consultation] ([LocalAreaId]);  
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportExporter_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Exporter]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportExporter_NotificationId
        ON [ImportNotification].[Exporter] ([ImportNotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportImporter_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Importer]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportImporter_NotificationId
        ON [ImportNotification].[Importer] ([ImportNotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportProducer_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Producer]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportProducer_NotificationId
        ON [ImportNotification].[Producer] ([ImportNotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportFacilityCollection_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[FacilityCollection]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportFacilityCollection_NotificationId
        ON [ImportNotification].[FacilityCollection] ([ImportNotificationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportFacility_FacilityCollectionId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Facility]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportFacility_FacilityCollectionId
        ON [ImportNotification].[Facility] ([FacilityCollectionId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportNotificationAssessment_NotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[NotificationAssessment]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportNotificationAssessment_NotificationId
        ON [ImportNotification].[NotificationAssessment] ([NotificationApplicationId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportNotificationDates_NotificationAssessmentId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[NotificationDates]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportNotificationDates_NotificationAssessmentId
        ON [ImportNotification].[NotificationDates] ([NotificationAssessmentId]); 
END

IF NOT EXISTS (
    SELECT 1 FROM SYS.INDEXES
    WHERE NAME = 'IX_ImportShipment_ImportNotificationId'
    AND OBJECT_ID = OBJECT_ID('[ImportNotification].[Shipment]'))
BEGIN
    CREATE NONCLUSTERED INDEX IX_ImportShipment_ImportNotificationId
        ON [ImportNotification].[Shipment] ([ImportNotificationId]); 
END