ALTER TABLE [Lookup].[LocalArea]
ADD [InternalId] INT NULL;
GO

-- New area names
UPDATE [Lookup].[LocalArea]
SET [Name] = 'East Midlands',
	[InternalId] = 50
WHERE [Name] = 'Derbyshire, Nottinghamshire and Leicestershire';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'West Midlands',
	[InternalId] = 51
WHERE [Name] = 'Staffordshire, Warwickshire and West Midlands';

UPDATE [Lookup].[LocalArea]
SET [InternalId] = 3
WHERE [Name] = 'Lincolnshire and Northamptonshire';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'East Anglia',
	[InternalId] = 52
WHERE [Name] = 'Cambridgeshire and Befordshire';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'North East',
	[InternalId] = 53
WHERE [Name] = 'Northumberland, Durham and Tees';

UPDATE [Lookup].[LocalArea]
SET [InternalId] = 34
WHERE [Name] = 'Yorkshire';

UPDATE [Lookup].[LocalArea]
SET [InternalId] = 12
WHERE [Name] = 'Greater Manchester, Merseyside and Cheshire';

UPDATE [Lookup].[LocalArea]
SET	[InternalId] = 11
WHERE [Name] = 'Cumbria and Lancashire';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'Devon, Cornwall and the Isles of Scilly',
	[InternalId] = 54
WHERE [Name] = 'Devon and Cornwall';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'Wessex',
	[InternalId] = 28
WHERE [Name] = 'South West Wessex';

UPDATE [Lookup].[LocalArea]
SET [InternalId] = 39
WHERE [Name] = 'Solent and South Downs';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'Kent, South London and East Sussex',
	[InternalId] = 55
WHERE [Name] = 'Kent and South London';

UPDATE [Lookup].[LocalArea]
SET [Name] = 'Thames',
	[InternalId] = 56
WHERE [Name] = 'West Thames';

UPDATE [Lookup].[LocalArea]
SET [InternalId] = 36
WHERE [Name] = 'Hertfordshire and North London';

GO

-- Combine area assignments
DECLARE @westMidlands UNIQUEIDENTIFIER;
DECLARE @shropshire UNIQUEIDENTIFIER;
DECLARE @eastAnglia UNIQUEIDENTIFIER;
DECLARE @essex UNIQUEIDENTIFIER;

SELECT @westMidlands = [Id]
FROM [Lookup].[LocalArea]
WHERE [Name] = 'West Midlands';

SELECT @eastAnglia = [Id]
FROM [Lookup].[LocalArea]
WHERE [Name] = 'East Anglia';

SELECT @shropshire = [Id]
FROM [Lookup].[LocalArea]
WHERE [Name] = 'Shropshire, Herefordshire, Worcestershire and Gloucestershire';

SELECT @essex = [Id]
FROM [Lookup].[LocalArea]
WHERE [Name] = 'Essex, Norfolk and Suffolk';

UPDATE [Notification].[Consultation]
SET [LocalAreaId] = @westMidlands
WHERE [LocalAreaId] = @shropshire;

UPDATE [Notification].[Consultation]
SET [LocalAreaId] = @eastAnglia
WHERE [LocalAreaId] = @essex;

UPDATE [ImportNotification].[Consultation]
SET [LocalAreaId] = @westMidlands
WHERE [LocalAreaId] = @shropshire;

UPDATE [ImportNotification].[Consultation]
SET [LocalAreaId] = @eastAnglia
WHERE [LocalAreaId] = @essex;

UPDATE [Person].[InternalUser]
SET [LocalAreaId] = @westMidlands
WHERE [LocalAreaId] = @shropshire;

UPDATE [Person].[InternalUser]
SET [LocalAreaId] = @eastAnglia
WHERE [LocalAreaId] = @essex;

GO

-- Remove combined
DELETE FROM [Lookup].[LocalArea]
WHERE [Name] = 'Shropshire, Herefordshire, Worcestershire and Gloucestershire';

DELETE FROM [Lookup].[LocalArea]
WHERE [Name] = 'Essex, Norfolk and Suffolk';

GO