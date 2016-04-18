Go
ALTER TABLE [Notification].[RecoveryInfo]
DROP COLUMN DisposalAmount, DisposalUnit

GO

ALTER TABLE [Notification].[Notification]
DROP COLUMN MethodOfDisposal

GO