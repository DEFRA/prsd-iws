ALTER SCHEMA [Notification]
TRANSFER [Lookup].[EntryOrExitPoint]

ALTER TABLE [Notification].[EntryOrExitPoint]
ADD RowVersion ROWVERSION NOT NULL