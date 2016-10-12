CREATE SCHEMA [FileStore]
    AUTHORIZATION [dbo];
GO

CREATE TABLE [FileStore].[File] (
    [Id]       UNIQUEIDENTIFIER CONSTRAINT [PK_File] PRIMARY KEY       NOT NULL,
    [Name]     VARCHAR(256)            NOT NULL,
    [Type]     VARCHAR(64)                     NOT NULL,
    [Content]     VARBINARY(MAX)          NOT NULL
) ON [FileStore];