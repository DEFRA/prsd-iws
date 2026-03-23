ALTER TABLE [Lookup].[OperationCode] ADD [Description] NVARCHAR(250) NOT NULL DEFAULT '';
GO

ALTER TABLE [Lookup].[OperationCode] ADD [NotificationType] INT NOT NULL DEFAULT 1;
GO

--1 = R1
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Use as a fuel (other than in a direct incineration) or other means to generate energy (Basel/OECD) – Use principally as a fuel or other means to generate energy (EU)',
		[NotificationType] = 1
WHERE Id = 1;

--2 = R2
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Solvent reclamation/regeneration',
		[NotificationType] = 1
WHERE Id = 2;

--3 = R3
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Recycling/reclamation of organic substances which are not used as solvents',
		[NotificationType] = 1
WHERE Id = 3;

--4 = R4
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Recycling/reclamation of metals and metal compounds',
		[NotificationType] = 1
WHERE Id = 4;

--5 = R5
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Recycling/reclamation of other inorganic materials',
		[NotificationType] = 1
WHERE Id = 5;

--6 = R6
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Regeneration of acids or bases',
		[NotificationType] = 1
WHERE Id = 6;

--7 = R7
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Recovery of components used for pollution abatement',
		[NotificationType] = 1
WHERE Id = 7;

--8 = R8
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Recovery of components from catalysts',
		[NotificationType] = 1
WHERE Id = 8;

--9 = R9
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Used oil re-refining or other reuses of previously used oil',
		[NotificationType] = 1
WHERE Id = 9;

--10 = R10
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Land treatment resulting in benefit to agriculture or ecological improvement',
	[NotificationType] = 1
WHERE Id = 10;

--11 = R11
UPDATE [Lookup].[OperationCode] 
	SET [Description] = 'Uses of residual materials obtained from any of the operations numbered R1 - R10',
		[NotificationType] = 1
WHERE Id = 11;

--12 = R12
UPDATE [Lookup].[OperationCode] 
	SET [Description] = 'Exchange of wastes for submission to any of the operations numbered R1 - R11',
		[NotificationType] = 1
WHERE Id = 12;

--13 = R13
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Accumulation of material intended for any operation in this list',
		[NotificationType] = 1
WHERE Id = 13;

--14 = D1
UPDATE [Lookup].[OperationCode] 
	SET [Description] = 'Deposit into or onto land (e.g., landfill, etc.)',
		[NotificationType] = 2
WHERE Id = 14;

--15 = D2
UPDATE [Lookup].[OperationCode] 
	SET [Description] = 'Land treatment, (e.g., biodegradation of liquid or sludgy discards in soils, etc.)',
		[NotificationType] = 2
WHERE Id = 15;

--16 = D3
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Deep injection, (e.g., injection of pumpable discards into wells, salt domes or naturally occurring repositories, etc.)',
		[NotificationType] = 2
WHERE Id = 16;

--17 = D4
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Surface impoundment, (e.g., placement of liquid or sludge discards into pits, ponds or lagoons, etc.)',
		[NotificationType] = 2
WHERE Id = 17;

--18 = D5
UPDATE [Lookup].[OperationCode] 
	SET [Description] = 'Specially engineered landfill, (e.g., placement into lined discrete cells which are capped and isolated from one another and the environment, etc.)',
		[NotificationType] = 2
WHERE Id = 18;

--19 = D6
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Release into water body except seas / oceans',
		[NotificationType] = 2
WHERE Id = 19;

--20 = D7
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Release into seas/oceans including sea-bed insertion',
		[NotificationType] = 2
WHERE Id = 20;

--21 = D8
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Biological treatment not specified elsewhere in this list which results in final compounds or mixtures which are discarded by means of any of the operations in this list',
		[NotificationType] = 2
WHERE Id = 21;

--22 = D9
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Physico-chemical treatment not specified elsewhere in this list which results in final compounds or mixtures which are discarded by means of any of the operations in this list (e.g., evaporation, drying, calcination, etc.,)',
		[NotificationType] = 2
WHERE Id = 22;

--23 = D10
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Incineration on land',
		[NotificationType] = 2
WHERE Id = 23;

--24 = D11
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Incineration at sea',
		[NotificationType] = 2
WHERE Id = 24;

--25 = D12
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Permanent storage (e.g., emplacement of containers in a mine, etc.)',
		[NotificationType] = 2
WHERE Id = 25;

--26 = D13
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Blending or mixing prior to submission to any of the operations in this list',
		[NotificationType] = 2
WHERE Id = 26;

--27 = D14
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Repackaging prior to submission to any of the operations in this list',
		[NotificationType] = 2
WHERE Id = 27;

--28 = D15
UPDATE [Lookup].[OperationCode]
	SET [Description] = 'Storage pending any of the operations numbered in this list',
		[NotificationType] = 2
WHERE Id = 28;