UPDATE [Lookup].[CompetentAuthority]
SET IsTransitAuthority = NULL
WHERE CountryId NOT IN (SELECT CountryId FROM [Lookup].[CompetentAuthority] GROUP BY CountryId HAVING COUNT(1) > 1)

UPDATE [Lookup].[CompetentAuthority]
SET IsTransitAuthority = NULL
WHERE CountryId NOT IN (SELECT CountryId FROM [Lookup].[CompetentAuthority] WHERE IsTransitAuthority = 1)