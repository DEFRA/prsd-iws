/*	script to add Competent Authorities for missing countries
	data for countries which were not included in earlier script 
	file: 20150730-120000-insert-data-into-ca-entry-exit-point.sql */

INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Urban Affairs and Environment', 'AO', [Id] FROM [Lookup].[Country] Where [Name]='Angola'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Ecology and Natural Resources', 'AZ', [Id] FROM [Lookup].[Country] Where [Name]='Azerbaijan'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Department of Environment, Ministry of Forestry, Fisheries and Sustainable Development', 'BZ', [Id] FROM [Lookup].[Country] Where [Name]='Belize';
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'National Environment Commission Secretariat', 'BT', [Id] FROM [Lookup].[Country] Where [Name]='Bhutan'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Environment and Tourism', 'BA', [Id] FROM [Lookup].[Country] Where [Name]='Bosnia and Herzegovina'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Direction Générale de l''Environnement, Ministère de l''Environnement, de l''Habitation et de l''Aménagement du Territoire', 'CV', [Id] FROM [Lookup].[Country] Where [Name]='Cape Verde'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Département de l''Ecologie et de la Prévention des Risques, Ministère de l''Environnement de l''Écologie et du Developpement Durable', 'CF', [Id] FROM [Lookup].[Country] Where [Name]='Central African Republic';
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministère de l''Agriculture et de l''Environnement', 'TD', [Id] FROM [Lookup].[Country] Where [Name]='Chad'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Direction Générale de l''Environnement, Ministère du Tourisme', 'CG', [Id] FROM [Lookup].[Country] Where [Name]='Congo'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'National Environment Service', 'CK', [Id] FROM [Lookup].[Country] Where [Name]='Cook Islands'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministère de l''Habitat de l''Urbanisme et de l''Environnement', 'DJ', [Id] FROM [Lookup].[Country] Where [Name]='Djibouti'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministerio de Pesca y Medio Ambiente', 'GQ', [Id] FROM [Lookup].[Country] Where [Name]='Equatorial Guinea'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Department of Environment, Ministry of Land, Water and Environment', 'ER', [Id] FROM [Lookup].[Country] Where [Name]='Eritrea';
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Fiji', 'FJ', [Id] FROM [Lookup].[Country] Where [Name]='Fiji'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Grenada', 'GD', [Id] FROM [Lookup].[Country] Where [Name]='Grenada'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Secrétariat d''Etat de l''Environment et Tourism', 'GW', [Id] FROM [Lookup].[Country] Where [Name]='Guinea-Bissau'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environmental Protection Agency', 'GY', [Id] FROM [Lookup].[Country] Where [Name]='Guyana'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Direction Technique du Ministère de l''Environnement', 'HT', [Id] FROM [Lookup].[Country] Where [Name]='Haiti'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Environment', 'IQ', [Id] FROM [Lookup].[Country] Where [Name]='Iraq'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Energy', 'KZ', [Id] FROM [Lookup].[Country] Where [Name]='Kazakhstan'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Administration des Ressources en Eau et de L''Environnement, Water Resources and Environment Administration', 'LA', [Id] FROM [Lookup].[Country] Where [Name]='Lao People''s Democratic Republic'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environment Protection Agency', 'LR', [Id] FROM [Lookup].[Country] Where [Name]='Liberia'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Solid and Hazardous Waste Follow-up, Environment General Authority (EGA)', 'LY', [Id] FROM [Lookup].[Country] Where [Name]='Libya'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministre de l''Environnement, des Eaux et Forêts et du Tourisme', 'MG', [Id] FROM [Lookup].[Country] Where [Name]='Madagascar'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environmental Affairs Department, Ministry of Natural Resources, Energy and Mining', 'MW', [Id] FROM [Lookup].[Country] Where [Name]='Malawi'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Foreign Affairs', 'MH', [Id] FROM [Lookup].[Country] Where [Name]='Marshall Islands'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environmental Conservation Department, Ministry of Environmental Conservation and Forestry', 'MM', [Id] FROM [Lookup].[Country] Where [Name]='Myanmar'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Department of Commerce Industry and Environment', 'NR', [Id] FROM [Lookup].[Country] Where [Name]='Nauru'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Niue', 'NU', [Id] FROM [Lookup].[Country] Where [Name]='Niue'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Department of Environment Protection, Ministry of Land and Environment Protection', 'KP', [Id] FROM [Lookup].[Country] Where [Name]='Korea (Democratic People''s Republic of)'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environmental Quality Protection Board', 'PW', [Id] FROM [Lookup].[Country] Where [Name]='Palau'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Palestine, State of', 'PS', [Id] FROM [Lookup].[Country] Where [Name]='Palestine, State of'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Lands, Environment, Forestry, Water and Mines', 'RW', [Id] FROM [Lookup].[Country] Where [Name]='Rwanda'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Health and the Environment', 'VC', [Id] FROM [Lookup].[Country] Where [Name]='Saint Vincent and the Grenadines'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Natural Resources, Environment and Meteorology', 'WS', [Id] FROM [Lookup].[Country] Where [Name]='Samoa'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Département d’assainissement et qualité de l''environnement, Direção de Conservação, Saneamento e Qualidade de Ambiente', 'ST', [Id] FROM [Lookup].[Country] Where [Name]='Sao Tome and Principe'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Sierra Leone', 'SL', [Id] FROM [Lookup].[Country] Where [Name]='Sierra Leone'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Solomon Islands', 'SB', [Id] FROM [Lookup].[Country] Where [Name]='Solomon Islands'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of State for Environment', 'SO', [Id] FROM [Lookup].[Country] Where [Name]='Somalia'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Higher Council for Environment and Natural Resources', 'SD', [Id] FROM [Lookup].[Country] Where [Name]='Sudan'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'National Environmental Policy Department', 'SR', [Id] FROM [Lookup].[Country] Where [Name]='Suriname'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environmental Assessment and Compliance', 'SZ', [Id] FROM [Lookup].[Country] Where [Name]='Swaziland'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Tajikistan', 'TJ', [Id] FROM [Lookup].[Country] Where [Name]='Tajikistan'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Timor-Leste', 'TL', [Id] FROM [Lookup].[Country] Where [Name]='Timor-Leste'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Direction de l''Environnement, Ministère de l''Environnement et des Ressources Forestières', 'TG', [Id] FROM [Lookup].[Country] Where [Name]='Togo'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Ministry of Meteorology, Energy, Information, Disaster Management, Environement, Climate Change and Communications (MEIDECC)', 'TO', [Id] FROM [Lookup].[Country] Where [Name]='Tonga'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Tuvalu', 'TV', [Id] FROM [Lookup].[Country] Where [Name]='Tuvalu'; 
INSERT INTO [Lookup].[CompetentAuthority]([Id],[IsSystemUser],[Name],[Code],[CountryId]) SELECT NEWID(),0, 'Environmental Management Agency', 'ZW', [Id] FROM [Lookup].[Country] Where [Name]='Zimbabwe'; 

GO

/* To update CA code for DE005 - Germany */
IF EXISTS (SELECT 1 FROM [Lookup].[CompetentAuthority] WHERE [Code]='DE005' AND [Name]='Umweltbundesamt') 
BEGIN
	UPDATE [Lookup].[CompetentAuthority] SET [Code]='DE 005' WHERE [Code]='DE005' AND [Name]='Umweltbundesamt'
END
GO
