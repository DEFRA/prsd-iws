-- Financial Guarantee

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [ReleasedDate] DATE NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [CompletedDate] DATE NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [ReceivedDate] DATE NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [DecisionDate] DATE NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [ApprovedFrom] DATE NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [ApprovedTo] DATE NULL;

ALTER TABLE [Notification].[FinancialGuarantee]
ALTER COLUMN [CreatedDate] DATETIMEOFFSET(0) NULL;

-- Financial Guarantee Status Change

ALTER TABLE [Notification].[FinancialGuaranteeStatusChange]
DROP CONSTRAINT [DF_FinancialGuaranteeStatusChange_ChangeDate];

DROP INDEX [IX_FinancialGuaranteeStatusChange_ChangeDate] 
ON [Notification].[FinancialGuaranteeStatusChange];

DROP INDEX [IX_FinancialGuaranteeStatusChange_FinancialGuaranteeId] 
ON [Notification].[FinancialGuaranteeStatusChange];

ALTER TABLE [Notification].[FinancialGuaranteeStatusChange]
ALTER COLUMN [ChangeDate] DATETIMEOFFSET(0) NULL;

ALTER TABLE [Notification].[FinancialGuaranteeStatusChange] 
ADD CONSTRAINT [DF_FinancialGuaranteeStatusChange_ChangeDate] DEFAULT (GETUTCDATE()) FOR [ChangeDate];

CREATE NONCLUSTERED INDEX [IX_FinancialGuaranteeStatusChange_ChangeDate] 
ON [Notification].[FinancialGuaranteeStatusChange]
(
	[ChangeDate] ASC
);

CREATE NONCLUSTERED INDEX [IX_FinancialGuaranteeStatusChange_FinancialGuaranteeId] 
ON [Notification].[FinancialGuaranteeStatusChange]
(
	[FinancialGuaranteeId] ASC
);

-- Notification Dates

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [NotificationReceivedDate] DATE NULL;

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [PaymentReceivedDate] DATE NULL;

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [CommencementDate] DATE NULL;

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [CompleteDate] DATE NULL;

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [TransmittedDate] DATE NULL;

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [AcknowledgedDate] DATE NULL;

ALTER TABLE [Notification].[NotificationDates]
ALTER COLUMN [ConsentedDate] DATE NULL;

-- Movement Date History

ALTER TABLE [Notification].[MovementDateHistory]
ALTER COLUMN [PreviousDate] DATETIMEOFFSET(0) NOT NULL;

ALTER TABLE [Notification].[MovementDateHistory]
ALTER COLUMN [DateChanged] DATETIMEOFFSET(0) NOT NULL;

-- Movement Receipt

ALTER TABLE [Notification].[MovementReceipt]
ALTER COLUMN [Date] DATE NOT NULL;

ALTER TABLE [Notification].[MovementOperationReceipt]
ALTER COLUMN [Date] DATE NOT NULL;

-- Notification

ALTER TABLE [Notification].[Notification]
DROP CONSTRAINT [DF_Notification_CreatedDate];

ALTER TABLE [Notification].[Notification]
ALTER COLUMN [CreatedDate] DATETIMEOFFSET(0) NOT NULL;

ALTER TABLE [Notification].[Notification] 
ADD CONSTRAINT [DF_Notification_CreatedDate] DEFAULT (GETUTCDATE()) FOR [CreatedDate];
GO

-- Notification Status Change

ALTER TABLE [Notification].[NotificationStatusChange]
DROP CONSTRAINT [DF_NotificationStatusChange_ChangeDate];

ALTER TABLE [Notification].[NotificationStatusChange]
ALTER COLUMN [ChangeDate] DATETIMEOFFSET(0) NULL;

ALTER TABLE [Notification].[NotificationStatusChange] 
ADD CONSTRAINT [DF_NotificationStatusChange_ChangeDate] DEFAULT (GETUTCDATE()) FOR [ChangeDate];

-- Movement Status Change

ALTER TABLE [Notification].[MovementStatusChange]
ALTER COLUMN [ChangeDate] DATETIMEOFFSET(0) NOT NULL;

-- Import notification consent

ALTER TABLE [ImportNotification].[Consent]
ALTER COLUMN [From] DATE NOT NULL;

ALTER TABLE [ImportNotification].[Consent]
ALTER COLUMN [To] DATE NOT NULL;

-- Import notification movement

ALTER TABLE [ImportNotification].[Movement]
ALTER COLUMN [ActualShipmentDate] DATE NOT NULL;

ALTER TABLE [ImportNotification].[Movement]
ALTER COLUMN [PrenotificationDate] DATE NULL;

ALTER TABLE [ImportNotification].[MovementOperationReceipt]
ALTER COLUMN [Date] DATE NOT NULL;

ALTER TABLE [ImportNotification].[MovementReceipt]
ALTER COLUMN [Date] DATE NOT NULL;

ALTER TABLE [ImportNotification].[MovementRejection]
ALTER COLUMN [Date] DATE NOT NULL;

-- Import notification dates

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [NotificationReceivedDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [PaymentReceivedDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [WithdrawnDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [AssessmentStartedDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [NotificationCompletedDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [AcknowledgedDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [ConsentedDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [ConsentWithdrawnDate] DATE NULL;

ALTER TABLE [ImportNotification].[NotificationDates]
ALTER COLUMN [ObjectedDate] DATE NULL;

-- Notification movement

ALTER TABLE [Notification].[Movement]
ALTER COLUMN [PrenotificationDate] DATE NULL;

ALTER TABLE [Notification].[MovementRejection]
ALTER COLUMN [Date] DATE NOT NULL;

GO