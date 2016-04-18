

GO
PRINT N'Altering [Identity].[AspNetUsers]...';


GO
ALTER TABLE [Identity].[AspNetUsers]
    ADD [JobTitle] NVARCHAR (256) NULL,
        [CompetentAuthority] NVARCHAR (256) NULL,
        [LocalArea] NVARCHAR (256) NULL,
        [IsAdmin] BIT NOT NULL DEFAULT 0,
        [IsApproved] BIT NOT NULL DEFAULT 0;
GO
PRINT N'Update complete.';


GO
