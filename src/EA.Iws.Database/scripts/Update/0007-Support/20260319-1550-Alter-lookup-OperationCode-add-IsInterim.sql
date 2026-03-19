ALTER TABLE [Lookup].[OperationCode] ADD [IsInterim] BIT NOT NULL CONSTRAINT DF_OperationCode_IsInterim DEFAULT 0;

GO
--12	R12
--13	R13
--26	D13
--27	D14
--28	D15
UPDATE [Lookup].[OperationCode] SET [IsInterim] = 1 WHERE ID IN (12, 13, 26, 27, 28);