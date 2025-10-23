DECLARE @ValidFrom DATE;
DECLARE @CompetentAuthority INT;

SET @ValidFrom = '2025-10-01';
SET @CompetentAuthority = 1; --EA (England)

INSERT INTO [Lookup].[PricingStructure] (Id, CompetentAuthority, ShipmentQuantityRangeId, ActivityId, Price, PotentialRefund, ValidFrom) 
	VALUES 	 
	-- Export Recovery IsInterim 0 Range 1-5
	 ('7BB14B71-8A89-419C-B67B-98FCF8590C8A', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '75496653-C767-44D2-AA27-A4C9010901C7', '3679.00', '0.00', @ValidFrom),
	-- Export Recovery IsInterim 0 Range 6-20
	 ('4F0FC301-DE99-4B1D-B289-ECEBF08A3236', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '4145.00', '466.00', @ValidFrom),
	-- Export Recovery IsInterim 0 Range 21-100
	 ('A14A1C89-0D22-4DC7-BD0F-0482D0B4A5BC', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '5198.00', '1519.00', @ValidFrom),
	-- Export Recovery IsInterim 0 Range 101-300
	 ('7FF3C332-54B9-470B-9799-510F9D52E6C6', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '75496653-C767-44D2-AA27-A4C9010901C7', '7521.00', '3842.00', @ValidFrom),
	 -- Export Recovery IsInterim 0 Range 301-500
	 ('0C3BBDD9-78EA-42D9-B8BA-220763C250F9', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '75496653-C767-44D2-AA27-A4C9010901C7', '10895.00', '7216.00', @ValidFrom),
	-- Export Recovery IsInterim 0 Range 501-
	 ('D96BAD31-3816-49FF-BE7A-46DD8535010C', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '13637.00', '9958.00', @ValidFrom),

	-- Export Recovery IsInterim 1 Range 1--5
	 ('8D7EFC06-CF04-4A3A-90DC-5D84CF94CB41', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '3679.00', '0.00', @ValidFrom),
	-- Export Recovery IsInterim 1 Range 6-20
	 ('8297A771-9B61-46BF-83EF-F94EFB943DD0', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '4145.00', '466.00', @ValidFrom),
	-- Export Recovery IsInterim 1 Range 21-100
	 ('953CBBEE-5E25-448F-9D24-6C53E938A32C', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '5198.00', '1519.00', @ValidFrom),
	-- Export Recovery IsInterim 1 Range 101-300
	 ('D5825524-60B3-4AA2-B654-4A098570DDCC', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '7521.00', '3842.00', @ValidFrom),
	 -- Export Recovery IsInterim 1 Range 301-500
	 ('B1581656-6354-4A93-8248-530C2D89596B', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '10895.00', '7216.00', @ValidFrom),
	-- Export Recovery IsInterim 1 Range 501-
	 ('EF03D8C8-22EB-4D5C-8AD7-E413D7C96F28', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '13637.00', '9958.00', @ValidFrom),

	-- Export Disposal IsInterim 0 Range 1-5
	 ('A7217455-E00F-4C59-A348-C1207CE53F0A', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '4115.00', '0.00', @ValidFrom),
	-- Export Disposal IsInterim 0 Range 6-20
	 ('E3BB797E-A9D6-4D4D-9E0E-5F0A23FC54F1', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '4636.00', '521.00', @ValidFrom),
	-- Export Disposal IsInterim 0 Range 21-100
	 ('EAB75F27-D084-42C4-83D4-0A903DBEF387', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '5815.00', '1700.00', @ValidFrom),
	-- Export Disposal IsInterim 0 Range 101-300
	 ('7F574A61-E55B-44A9-A843-6C4B7E69EC0B', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '8413.00', '4298.00', @ValidFrom),
	 -- Export Disposal IsInterim 0 Range 301-500
	 ('BA0AD291-82C8-4B28-873D-EF0EE0BD1BB4', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '12187.00', '8072.00', @ValidFrom),
	-- Export Disposal IsInterim 0 Range 501-
	 ('02551DA7-67B2-4897-93F0-0DF9EFA9E215', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '15254.00', '11139.00', @ValidFrom),

	-- Export Disposal IsInterim 1 Range 1-5
	 ('FF62BF3C-183F-463B-88B5-92C0957309EB', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '4115.00', '0.00', @ValidFrom),
	-- Export Disposal IsInterim 1 Range 6-20
	 ('22BE94CC-4AED-44AA-8B7B-68254DBF1EA2', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '4636.00', '521.00', @ValidFrom),
	-- Export Disposal IsInterim 1 Range 21-100
	 ('05497C4F-421C-4A3D-A202-A09553B5BCB4', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '5815.00', '1700.00', @ValidFrom),
	-- Export Disposal IsInterim 1 Range 101-300
	 ('3774BDD3-3617-4E99-B670-F8831256EAEF', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '8413.00', '4298.00', @ValidFrom),
	 -- Export Disposal IsInterim 1 Range 301-500
	 ('7DAF8381-216F-465D-BB17-4F5A33F87AEF', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '12187.00', '8072.00', @ValidFrom),
	-- Export Disposal IsInterim 1 Range 501-1000
	 ('2EAEBC38-F1BA-449C-9945-C314D8C078EB', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '15254.00', '11139.00', @ValidFrom),

	 -- Import Recovery IsInterim 0 Range 1-5
	 ('8ED69FAE-4D7C-4D78-8851-5D145DBA02E9', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '3796.00', '0.00', @ValidFrom),
	-- Import Recovery IsInterim 0 Range 6-20
	 ('B7F4B3BB-7E3D-4046-9A69-4882AF389483', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '4262.00', '466.00', @ValidFrom),
	-- Import Recovery IsInterim 0 Range 21-100
	 ('B90FB204-D4E9-45B8-A2CC-5FBA760855D9', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '5316.00', '1520.00', @ValidFrom),
	-- Import Recovery IsInterim 0 Range 101-300
	 ('2C7E649A-F4D6-4C17-ABD0-1D48D8BFC492', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '7638.00', '3842.00', @ValidFrom),
	-- Import Recovery IsInterim 0 Range 301-500
	 ('3AAD18B4-0572-4E54-9819-6D6F90ECDF9D', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '11012.00', '7216.00', @ValidFrom),
	-- Import Recovery IsInterim 0 Range 501-
	 ('3C7DBC89-CEAF-42DE-8D07-F21B8705A553', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '13754.00', '9958.00', @ValidFrom),

	-- Import Recovery IsInterim 1 Range 1-5
	 ('71F030A5-E258-4437-9BF8-1F6AEE706716', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '3796.00', '0.00', @ValidFrom),
	-- Import Recovery IsInterim 1 Range 6-20
	 ('4BFA7042-A10B-4389-B3A3-C775846A7D33', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '4262.00', '466.00', @ValidFrom),
	-- Import Recovery IsInterim 1 Range 21-100
	 ('E965C37E-CE0A-4F0A-A987-FAFD684FEF5F', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '5316.00', '1520.00', @ValidFrom),
	-- Import Recovery IsInterim 1 Range 101-300
	 ('A70E2FA9-4B5B-4161-86D6-CF3C50C377A3', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '7638.00', '3842.00', @ValidFrom),
	-- Import Recovery IsInterim 1 Range 301-500
	 ('BB967558-8AE7-46B5-AF8B-0D2EDE191182', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '11012.00', '7216.00', @ValidFrom),
	-- Import Recovery IsInterim 1 Range 501-
	 ('8B1573BA-D8E7-4413-8F32-9CF767F17383', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '13754.00', '9958.00', @ValidFrom),

	-- Import Disposal IsInterim 0 Range 1-5
	 ('4E6C1750-7C68-4ABA-A074-BE97D4F3E702', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '3796.00', '0.00', @ValidFrom),
	-- Import Disposal IsInterim 0 Range 6-20
	 ('667E5BA9-3DF0-42B5-ABA1-7559B7121829', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '4262.00', '466.00', @ValidFrom),
	-- Import Disposal IsInterim 0 Range 21-100
	 ('103256C0-0DED-4A2D-B332-3C0CB1494994', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '5316.00', '1520.00', @ValidFrom),
	-- Import Disposal IsInterim 0 Range 101-300
	 ('48D3F48A-A805-4366-B0A9-18BD8FCF515B', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '7638.00', '3842.00', @ValidFrom),
	-- Import Disposal IsInterim 0 Range 301-500
	 ('6F13716A-1E50-48D6-A189-55792450BA45', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '11012.00', '7216.00', @ValidFrom),
	-- Import Disposal IsInterim 0 Range 501-
	 ('DB1FFED5-A2FD-4853-93F5-8BC505394A4A', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '13754.00', '9958.00', @ValidFrom),

	-- Import Disposal IsInterim 1 Range 1-5
	 ('C1B2084C-294F-4DEF-91F0-1ACFA6BCAC74', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '3796.00', '0.00', @ValidFrom),
	-- Import Disposal IsInterim 1 Range 6-20
	 ('1548C226-A527-49A5-8E07-6F73F711ECDE', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '4262.00', '466.00', @ValidFrom),
	-- Import Disposal IsInterim 1 Range 21-100
	 ('CE2FFAB1-0B3E-477F-8E77-DCC42E6A6C1F', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '5316.00', '1520.00', @ValidFrom),
	-- Import Disposal IsInterim 1 Range 101-300
	 ('6D6D5043-DE11-4ED6-A362-E122D28E44A7', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '7638.00', '3842.00', @ValidFrom),
	-- Import Disposal IsInterim 1 Range 301-500
	 ('5C3A2365-E883-4E15-981B-873090E3FC5B', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '11012.00', '7216.00', @ValidFrom),
	-- Import Disposal IsInterim 1 Range 501-
	 ('B18D73E6-87C2-4E32-A598-8B266BD11B16', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '13754.00', '9958.00', @ValidFrom);

--1 = EA Fixed Additional Charge for each data change
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 1, @ValidFrom, 93);

--2 = EA Import Disposal/Recovery custom per 100 (or part of) shipments over 1000 additional charge
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 2, @ValidFrom, 1376);

--3 = EA Export Recovery custom per 100 (or part of) shipments over 1000 additional charge
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 3, @ValidFrom, 1363);

--4 = EA Export Disposal custom per 100 (or part of) shipments over 1000 additional charge
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 4, @ValidFrom, 1525);

--11 = Single ship
INSERT INTO [Lookup].[PricingFixedFee] ([Id], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom], [CompetentAuthority])
	VALUES ('D4C14EC4-582C-432C-80C4-C24D757092A4', NULL, 11, 9334, @ValidFrom, @CompetentAuthority);

--12 = Platform/Rig
INSERT INTO [Lookup].[PricingFixedFee] ([Id], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom], [CompetentAuthority])
	VALUES ('D1C7CE5A-9E3A-45A7-B21D-D7284504D5E4', NULL, 12, 9334, @ValidFrom, @CompetentAuthority);

--1 = Mercury
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('E6596ED3-408A-4E0D-A0D9-2DD6C5F0E5AD', 1, NULL, 327, @ValidFrom, @CompetentAuthority);

--2 = FGas
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('86904E82-4AD3-4537-9963-B7555F57B9D0', 2, NULL, 327, @ValidFrom, @CompetentAuthority);

--3 = NORM
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('5B4F2728-18ED-4B83-A02D-54F73554E131', 3, NULL, 327, @ValidFrom, @CompetentAuthority);

--4 = ODS
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('387DC131-1FFB-4D2F-A77B-38A7E16D2D9A', 4, NULL, 327, @ValidFrom, @CompetentAuthority);