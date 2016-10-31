-- Missing foreign key
ALTER TABLE [ImportNotification].[NotificationAssessment]
ADD CONSTRAINT FK_ImportNotificationAssessment_NotificationApplicationId
FOREIGN KEY (NotificationApplicationId)
REFERENCES [ImportNotification].[Notification] (Id);

-- Export

CREATE NONCLUSTERED INDEX IX_Movement_NotificationId
    ON [Notification].[Movement] ([NotificationId]); 


CREATE NONCLUSTERED INDEX IX_MovementReceipt_MovementId
    ON [Notification].[MovementReceipt] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_MovementOperationReceipt_MovementId
    ON [Notification].[MovementOperationReceipt] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_MovementRejection_MovementId
    ON [Notification].[MovementRejection] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_MovementDetails_MovementId
    ON [Notification].[MovementDetails] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_Consent_NotificationId
    ON [Notification].[Consent] ([NotificationApplicationId]); 


CREATE NONCLUSTERED INDEX IX_Consultation_NotificationId
    ON [Notification].[Consultation] ([NotificationId]);


CREATE NONCLUSTERED INDEX IX_Consultation_LocalAreaId
    ON [Notification].[Consultation] ([LocalAreaId]);  


CREATE NONCLUSTERED INDEX IX_FacilityCollection_NotificationId
    ON [Notification].[FacilityCollection] ([NotificationId]); 


CREATE NONCLUSTERED INDEX IX_ProducerCollection_NotificationId
    ON [Notification].[ProducerCollection] ([NotificationId]); 


CREATE NONCLUSTERED INDEX IX_Producer_ProducerCollectionId
    ON [Notification].[Producer] ([ProducerCollectionId]); 


CREATE NONCLUSTERED INDEX IX_Facility_FacilityCollectionId
    ON [Notification].[Facility] ([FacilityCollectionId]); 


CREATE NONCLUSTERED INDEX IX_NotificationDates_NotificationAssessmentId
    ON [Notification].[NotificationDates] ([NotificationAssessmentId]); 


CREATE NONCLUSTERED INDEX IX_ShipmentInfo_NotificationId
    ON [Notification].[ShipmentInfo] ([NotificationId]); 

-- Import

CREATE NONCLUSTERED INDEX IX_ImportMovement_NotificationId
    ON [ImportNotification].[Movement] ([NotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportMovementReceipt_MovementId
    ON [ImportNotification].[MovementReceipt] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_ImportMovementOperationReceipt_MovementId
    ON [ImportNotification].[MovementOperationReceipt] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_ImportMovementRejection_MovementId
    ON [ImportNotification].[MovementRejection] ([MovementId]); 


CREATE NONCLUSTERED INDEX IX_ImportConsent_NotificationId
    ON [ImportNotification].[Consent] ([NotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportConsultation_NotificationId
    ON [ImportNotification].[Consultation] ([NotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportConsultation_LocalAreaId
    ON [ImportNotification].[Consultation] ([LocalAreaId]);  


CREATE NONCLUSTERED INDEX IX_ImportExporter_NotificationId
    ON [ImportNotification].[Exporter] ([ImportNotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportImporter_NotificationId
    ON [ImportNotification].[Importer] ([ImportNotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportProducer_NotificationId
    ON [ImportNotification].[Producer] ([ImportNotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportFacilityCollection_NotificationId
    ON [ImportNotification].[FacilityCollection] ([ImportNotificationId]); 


CREATE NONCLUSTERED INDEX IX_ImportFacility_FacilityCollectionId
    ON [ImportNotification].[Facility] ([FacilityCollectionId]); 


CREATE NONCLUSTERED INDEX IX_ImportNotificationAssessment_NotificationId
    ON [ImportNotification].[NotificationAssessment] ([NotificationApplicationId]); 


CREATE NONCLUSTERED INDEX IX_ImportNotificationDates_NotificationAssessmentId
    ON [ImportNotification].[NotificationDates] ([NotificationAssessmentId]); 


CREATE NONCLUSTERED INDEX IX_ImportShipment_ImportNotificationId
    ON [ImportNotification].[Shipment] ([ImportNotificationId]); 