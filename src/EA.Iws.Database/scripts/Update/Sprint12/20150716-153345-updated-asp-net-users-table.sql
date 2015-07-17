

GO
PRINT N'Altering [Identity].[AspNetUsers]...';


GO
ALTER TABLE [Identity].[AspNetUsers]
    ADD [JobTitle] NVARCHAR (256) NULL,
        [CompetentAuthority] NVARCHAR (256) NULL,
        [LocalArea] NVARCHAR (256) NULL;

GO
PRINT N'Update complete.';


GO
