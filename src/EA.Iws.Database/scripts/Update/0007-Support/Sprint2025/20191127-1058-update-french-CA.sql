UPDATE [Lookup].[CompetentAuthority]
SET Name = 'PNTTD - Pôle National des Transferts Transfrontaliers de Déchets',
Code = 'F'
FROM [Lookup].[CompetentAuthority]
where Id = (SELECT [Id]  FROM [Lookup].[CompetentAuthority] where Code = 'FR')
