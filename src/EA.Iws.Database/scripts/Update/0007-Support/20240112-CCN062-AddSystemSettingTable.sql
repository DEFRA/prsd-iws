PRINT N'Creating [Utility].[SystemSettings]...';

GO
CREATE TABLE [Utility].[SystemSettings] (
    [Id] INT NOT NULL,
	[Value] NVARCHAR(256) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_SystemSettings] PRIMARY KEY CLUSTERED ([Id] ASC)
);

PRINT N'Adding entries to [Utility].[SystemSettings]...';

INSERT INTO [Utility].[SystemSettings] ([Id] ,[Value] ,[Description])
	VALUES 
		(1, '2024-04-01', 'New EA charge matrix ValidFrom date'),
		(2, '2024-04-01', 'New SEPA charge matrix ValidFrom date');
GO