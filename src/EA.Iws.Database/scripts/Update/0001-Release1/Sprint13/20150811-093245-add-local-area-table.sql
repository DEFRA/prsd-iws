GO
PRINT N'Creating [Lookup].[LocalArea]...';


GO
CREATE TABLE [Lookup].[LocalArea] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (1024)  NOT NULL,
    CONSTRAINT [PK_LocalArea_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

GO


INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Derbyshire, Nottinghamshire and Leicestershire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Staffordshire, Warwickshire and West Midlands')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Shropshire, Herefordshire, Worcestershire and Gloucestershire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Lincolnshire and Northamptonshire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Essex, Norfolk and Suffolk')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Cambridgeshire and Befordshire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Northumberland, Durham and Tees')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Yorkshire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Greater Manchester, Merseyside and Cheshire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Cumbria and Lancashire')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Devon and Cornwall')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'South West Wessex')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Solent and South Downs')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Kent and South London')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'West Thames')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Hertfordshire and North London')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'International Waste Shipments')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Scottish Environment Protection Agency')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'National Resources Wales')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Northern Ireland Environment Agency')
INSERT INTO [Lookup].[LocalArea]([Id],[Name])VALUES(newid(),'Head Office')


GO
PRINT N'Complete...';