ALTER TABLE [Lookup].[CompetentAuthority]
ADD [IsTransitAuthority] BIT NULL;

GO

UPDATE [Lookup].[CompetentAuthority]
SET [IsTransitAuthority] = 1
WHERE CODE IN (
'FR999',
'BE004',
'DE 005'
);

GO

UPDATE [Lookup].[CompetentAuthority]
SET [IsTransitAuthority] = 1
WHERE Name IN (
'Ministero dell''Ambiente e della Tutela del Territorio',
'Subdirección General de Producción y'
);

GO

UPDATE CA 
SET CA.[IsTransitAuthority] = 0
FROM [Lookup].[CompetentAuthority] CA
INNER JOIN [Lookup].[Country] C ON CA.CountryId = C.Id
where C.Name in (
'Germany',
'France',
'Spain',
'Italy',
'Belgium'
)
AND CA.[IsTransitAuthority] IS NULL;

GO