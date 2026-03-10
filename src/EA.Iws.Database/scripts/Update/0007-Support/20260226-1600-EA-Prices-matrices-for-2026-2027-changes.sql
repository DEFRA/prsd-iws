DECLARE @ValidFrom DATE;
DECLARE @CompetentAuthority INT;

SET @ValidFrom = '2026-04-01';
SET @CompetentAuthority = 1; --EA (England)

INSERT INTO [Lookup].[PricingStructure] (Id, CompetentAuthority, ShipmentQuantityRangeId, ActivityId, Price, PotentialRefund, ValidFrom) 
	VALUES 	 
	 -- Export Recovery Non Interim
	 ('A369F2D6-D0DD-4104-B6DC-9F5F5151A3A9', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '75496653-C767-44D2-AA27-A4C9010901C7', '3818.57', '0.00', @ValidFrom), -- Export Recovery IsInterim 0 Range 1-5
	 ('586AFAC2-0CC9-4D24-88A8-8C043864C80E', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '4302.55', '483.98', @ValidFrom),-- Export Recovery IsInterim 0 Range 6-20
	 ('215F45CC-5601-4639-B915-1D81C3D8D4AE', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '5395.94', '1577.37', @ValidFrom),-- Export Recovery IsInterim 0 Range 21-100
	 ('C7366F38-872D-4B2D-9F3F-82FF506CC854', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '75496653-C767-44D2-AA27-A4C9010901C7', '7806.36', '3987.79', @ValidFrom),-- Export Recovery IsInterim 0 Range 101-300
	 ('E6BF9795-29A8-422A-A783-285303205542', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '75496653-C767-44D2-AA27-A4C9010901C7', '11308.99', '7490.42', @ValidFrom), -- Export Recovery IsInterim 0 Range 301-500
	 ('534415A1-3A45-416F-9407-CE49B1800E9C', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '14154.87', '10336.30', @ValidFrom),-- Export Recovery IsInterim 0 Range 501-
	
	 -- Export Recovery Interim
	 ('C1EC08F1-307C-40B4-9F70-03BF602A32D2', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '3818.57', '0.00', @ValidFrom),-- Export Recovery IsInterim 1 Range 1--5
	 ('A3837D74-D66E-4499-B20F-67064C75BDB2', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '4302.55', '483.00', @ValidFrom),-- Export Recovery IsInterim 1 Range 6-20
	 ('41AF785C-2A75-4E9F-8740-0F02159701EB', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '5395.94', '1577.37', @ValidFrom),-- Export Recovery IsInterim 1 Range 21-100
	 ('E5BB3753-67BB-4D54-B8BE-A46AB84C0F9A', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '7806.36', '3987.79', @ValidFrom),-- Export Recovery IsInterim 1 Range 101-300
	 ('AFD3688F-2444-4ED4-9EC7-93772BFB9D64', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '11308.99', '7490.42', @ValidFrom),-- Export Recovery IsInterim 1 Range 301-500
	 ('F056EB25-8995-4E78-B262-BB915654C1F0', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '14154.87', '10336.30', @ValidFrom),-- Export Recovery IsInterim 1 Range 501-

	 -- Export Disposal Non Interim
	 ('DF33326B-7D02-41AA-8520-5EB772E77DDD', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '4271.79', '0.00', @ValidFrom),-- Export Disposal IsInterim 0 Range 1-5
	 ('81F6F2B7-9EBE-4B88-A841-B53190C4FFA3', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '4812.56', '540.78', @ValidFrom),-- Export Disposal IsInterim 0 Range 6-20
	 ('02C5F268-81FE-4DD0-96EF-0298B2E3C6E4', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '6036.12', '1764.33', @ValidFrom),-- Export Disposal IsInterim 0 Range 21-100
	 ('B6684713-BC4C-4C5E-854F-AF5598984C13', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '8732.90', '4461.12', @ValidFrom),-- Export Disposal IsInterim 0 Range 101-300
	 ('7DE99E19-EF57-462E-9A7F-49E81107969E', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '12649.69', '8377.91', @ValidFrom), -- Export Disposal IsInterim 0 Range 301-500
	 ('4C5207FD-32EC-428B-BE92-D40155CCB361', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '15834.00', '11562.22', @ValidFrom),-- Export Disposal IsInterim 0 Range 501-

	 -- Export Disposal Interim
	 ('D9CF895A-688B-42FC-99E9-33C4F4FC9A67', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '4271.79', '0.00', @ValidFrom),-- Export Disposal IsInterim 1 Range 1-5
	 ('AA730D3B-B1D3-4CE4-9C83-BB1B6063F9B2', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '4812.56', '540.78', @ValidFrom),-- Export Disposal IsInterim 1 Range 6-20
	 ('3783A362-353B-4BFC-8F88-F9C32F23A60D', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '6036.12', '1764.33', @ValidFrom),-- Export Disposal IsInterim 1 Range 21-100
	 ('81947122-8AC8-4BCB-96C5-9DDD063C03DD', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '8732.90', '4461.12', @ValidFrom),-- Export Disposal IsInterim 1 Range 101-300
	 ('21415D72-5C01-4DDB-A173-29F12EEA454C', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '12649.69', '8377.91', @ValidFrom),-- Export Disposal IsInterim 1 Range 301-500
	 ('1FAD0683-7038-4F8F-9DCA-0A56159F1E30', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '15834.00', '11562.22', @ValidFrom),-- Export Disposal IsInterim 1 Range 501-1000

	 -- Import Recovery Non Interim
	 ('E22FB5BF-BA91-414A-9462-10FA6E7DB9D1', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '3940.46', '0.00', @ValidFrom), -- Import Recovery IsInterim 0 Range 1-5
	 ('603DEA78-D8F0-443A-A325-3C616E868ED6', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '4424.43', '540.78', @ValidFrom),-- Import Recovery IsInterim 0 Range 6-20
	 ('1F5F2C92-8849-4107-AE0F-15771BD37042', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '5517.82', '7928.24', @ValidFrom),-- Import Recovery IsInterim 0 Range 21-100
	 ('00106BD0-79BF-421A-95CD-7D40C8BA5EC3', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '7928.24', '3987.79', @ValidFrom),-- Import Recovery IsInterim 0 Range 101-300
	 ('0A80856C-74DE-4006-8D0B-9B345B72F37F', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '11430.87', '7490.42', @ValidFrom),-- Import Recovery IsInterim 0 Range 301-500
	 ('0CB20A13-2DAE-4BD8-BBF6-B27A1BDDD5B2', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '14276.76', '10336.30', @ValidFrom),-- Import Recovery IsInterim 0 Range 501-

	 -- Import Recovery Interim
	 ('4E759797-4E57-41E2-BC5D-F69706955EC3', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '3940.46', '0.00', @ValidFrom),-- Import Recovery IsInterim 1 Range 1-5
	 ('63509285-59DD-4900-A886-C3A28853EC64', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '4424.43', '540.78', @ValidFrom),-- Import Recovery IsInterim 1 Range 6-20
	 ('BDC8D482-23C6-4799-A470-F3F2EE0D6E0A', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '5517.82', '7928.24', @ValidFrom),-- Import Recovery IsInterim 1 Range 21-100
	 ('38A2858D-BB8D-4B6C-A731-F17D2462CE64', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '7928.24', '3987.79', @ValidFrom),-- Import Recovery IsInterim 1 Range 101-300
	 ('877B15C9-9967-442C-9DBC-097C6B5CA95D', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '11430.87', '7490.42', @ValidFrom),	-- Import Recovery IsInterim 1 Range 301-500
	 ('E1A6CFE2-49F4-4CE3-B399-6AF603F686CE', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '14276.76', '10336.30', @ValidFrom),-- Import Recovery IsInterim 1 Range 501-
	
	 -- Import Disposal Non Interim
	 ('CDC34F22-D007-4870-AF00-6A93AE43037F', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '3940.46', '0.00', @ValidFrom),-- Import Disposal IsInterim 0 Range 1-5
	 ('320167CA-797D-43C9-AEAD-AC4A6F9E441B', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '4424.43', '540.78', @ValidFrom),-- Import Disposal IsInterim 0 Range 6-20
	 ('2F255AE1-2196-4F81-B098-88048A345A59', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '5517.82', '7928.24', @ValidFrom),-- Import Disposal IsInterim 0 Range 21-100
	 ('7752AD22-2AFB-4D4C-A479-CC3C1F2FD9D0', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '7928.24', '3987.79', @ValidFrom),-- Import Disposal IsInterim 0 Range 101-300
	 ('FF80503A-1E98-41C5-BADF-34F33E96B742', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '11430.87', '7490.42', @ValidFrom),-- Import Disposal IsInterim 0 Range 301-500
	 ('07246B7E-B720-477A-B969-A58E5CA8A4DF', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '14276.76', '10336.30',@ValidFrom),-- Import Disposal IsInterim 0 Range 501-

	 -- Import Disposal Interim
	 ('8CF06323-07FB-406A-9AB9-0617563D523D', @CompetentAuthority, '87443EEB-31F5-423F-905A-33ABE23F01F6', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '3940.46', '0.00', @ValidFrom),	-- Import Disposal IsInterim 1 Range 1-5
	 ('F12D2243-17D1-4E37-A868-769F745CC2FA', @CompetentAuthority, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '4424.43', '540.78', @ValidFrom),-- Import Disposal IsInterim 1 Range 6-20
	 ('B673876E-02B0-41B0-9698-3F3565679D18', @CompetentAuthority, '85270610-00B0-4C09-A1DB-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '5517.82', '7928.24', @ValidFrom),-- Import Disposal IsInterim 1 Range 21-100
	 ('9E4E1018-D2AE-47F6-AF97-7C0A09147FF0', @CompetentAuthority, '42ECE92D-6470-4B42-BA6A-D8E7FE65A402', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '7928.24', '3987.79', @ValidFrom),-- Import Disposal IsInterim 1 Range 101-300
	 ('9993183C-BFE3-430B-B232-DF881134AF14', @CompetentAuthority, '8810022E-11D4-4C25-880B-185787F65053', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '11430.87', '7490.42', @ValidFrom),	-- Import Disposal IsInterim 1 Range 301-500
	 ('5B5307F6-9911-4F39-97F5-3EC21BACEEE6', @CompetentAuthority, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '14276.76', '10336.30', @ValidFrom);-- Import Disposal IsInterim 1 Range 501-

--1 = EA Fixed Additional Charge for each data change
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 1, @ValidFrom, 97);

--2 = EA Import Disposal/Recovery custom per 100 (or part of) shipments over 1000 additional charge
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 2, @ValidFrom, 1428);

--3 = EA Export Recovery custom per 100 (or part of) shipments over 1000 additional charge
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 3, @ValidFrom, 1415);

--4 = EA Export Disposal custom per 100 (or part of) shipments over 1000 additional charge
INSERT INTO [Lookup].[SystemSettings] ([CompetentAuthority] , [PriceType], [ValidFrom] ,[Price])
	VALUES (@CompetentAuthority, 4, @ValidFrom, 1583);

--11 = Single ship
INSERT INTO [Lookup].[PricingFixedFee] ([Id], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom], [CompetentAuthority])
	VALUES ('89B21D05-9F5C-4BD7-8765-E2D6D6C11211', NULL, 11, 9689, @ValidFrom, @CompetentAuthority);

--12 = Platform/Rig
INSERT INTO [Lookup].[PricingFixedFee] ([Id], [WasteComponentTypeId], [WasteCategoryTypeId], [Price], [ValidFrom], [CompetentAuthority])
	VALUES ('6D4E9C52-0589-408C-9E7F-2BFD33D65A3B', NULL, 12, 9689, @ValidFrom, @CompetentAuthority);

--1 = Mercury
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('A8E8AE73-1C23-4BF3-80B5-28C9E0A90AAD', 1, NULL, 339, @ValidFrom, @CompetentAuthority);

--2 = FGas
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('77E350DA-CD06-48A7-84B9-3CB983420A3D', 2, NULL, 339, @ValidFrom, @CompetentAuthority);

--3 = NORM
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('229C0D3D-EBB5-4576-8372-816C3F1CA077', 3, NULL, 339, @ValidFrom, @CompetentAuthority);

--4 = ODS
INSERT INTO [Lookup].[PricingFixedFee] ([Id] ,[WasteComponentTypeId] ,[WasteCategoryTypeId] ,[Price] ,[ValidFrom], [CompetentAuthority])
	VALUES ('CAAC6903-81A3-4A1A-9127-3E30AF35ED66', 4, NULL, 339, @ValidFrom, @CompetentAuthority);