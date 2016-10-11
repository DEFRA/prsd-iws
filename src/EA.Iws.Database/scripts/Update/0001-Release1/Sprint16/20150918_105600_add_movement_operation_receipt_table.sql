CREATE TABLE [Notification].[MovementOperationReceipt] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [MovementReceiptId]   UNIQUEIDENTIFIER NOT NULL,
    [Date]         DATETIME         NOT NULL,
    [RowVersion]   ROWVERSION       NOT NULL,
    CONSTRAINT [PK_MovementOperationReceipt] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MovementOperationReceipt_MovementReceipt] FOREIGN KEY ([MovementReceiptId]) REFERENCES [Notification].[MovementReceipt] ([Id])
);