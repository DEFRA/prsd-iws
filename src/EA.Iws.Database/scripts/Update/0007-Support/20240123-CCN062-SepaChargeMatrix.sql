﻿GO

INSERT INTO [Lookup].[SystemSettings] (Id, [Value], [Description])
VALUES (3, '25', 'SEPA Additional Charge per shipment for self entering data')

--Add New SEPA charge matrix
DECLARE @validFrom nvarchar(100);
SET @validFrom = (SELECT [Value] from [Lookup].[SystemSettings] where Id = 2);

INSERT INTO [Lookup].[PricingStructure] (Id, CompetentAuthority, ShipmentQuantityRangeId, ActivityId, Price, PotentialRefund, ValidFrom) 
	VALUES
	-- Export Recovery IsInterim 0 Range 1-1
	 ('103DF727-889C-4C88-B4F4-F21035B15AF2', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '2600.00', '0.00', @validFrom),
	-- Export Recovery IsInterim 0 Range 2-5
	 ('58E820FC-68DD-4B47-980B-A470545C7AD6', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '2800.00', '0.00', @validFrom),
	-- Export Recovery IsInterim 0 Range 6-20
	 ('4A307D76-C123-4B3A-9615-6CC48F4648D2', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '3700.00', '900.00', @validFrom),
	-- Export Recovery IsInterim 0 Range 21-100
	 ('5FEBB1DC-B13D-4341-A6C9-D2741EBD931F', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '5070.00', '2270.00', @validFrom),	
	-- Export Recovery IsInterim 0 Range 101-500
	 ('2CC66775-491A-4671-BC40-290D485C7D0E', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '8920.00', '6120.00', @validFrom),
	-- Export Recovery IsInterim 0 Range 501-
	 ('C16DCBD6-900B-4F07-A378-C298944A2264', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '75496653-C767-44D2-AA27-A4C9010901C7', '15380.00', '12580.00', @validFrom),
	 
	-- Export Recovery IsInterim 1 Range 1-1
	 ('BA246A09-307A-4C74-A3B1-77D7C8D58890', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '3100.00', '0.00', @validFrom),
	-- Export Recovery IsInterim 1 Range 2-5
	 ('F5C766C4-E71A-4DCD-BF6A-3EB2E46FDFD3', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '3300.00', '0.00', @validFrom),
	-- Export Recovery IsInterim 1 Range 6-20
	 ('3F5D4E83-9D84-4792-BFC7-F6179FB6C124', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '4700.00', '1400.00', @validFrom),
	-- Export Recovery IsInterim 1 Range 21-100
	 ('E5CF8AB3-33CF-4A96-B67B-C982BFA94E3A', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '6070.00', '2770.00', @validFrom),	
	-- Export Recovery IsInterim 1 Range 101-500
	 ('B3EBCA1F-BBC0-4DD0-9DBE-E1201AE195A1', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '9920.00', '6620.00', @validFrom),
	-- Export Recovery IsInterim 1 Range 501-
	 ('6400DD74-C074-497D-B22F-0C4CF70AD42F', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '71CC7688-63D3-4312-BAD2-A4C9010901C7', '16380.00', '13080.00', @validFrom),

	-- Export Disposal IsInterim 0 Range 1-1
	 ('81795E72-3AFE-4634-868B-353EA4CACFC4', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '2800.00', '0.00', @validFrom),
	-- Export Disposal IsInterim 0 Range 2-5
	 ('F8D19848-DEB1-4F73-96C6-8142B3699D34', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '3000.00', '0.00', @validFrom),
	-- Export Disposal IsInterim 0 Range 6-20
	 ('5917FCB0-7353-40D6-BFCC-C8C8430285EC', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '4330.00', '1330.00', @validFrom),
	-- Export Disposal IsInterim 0 Range 21-100
	 ('B03DFEAD-2086-4BA5-8CCD-2E54106F5490', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '6500.00', '3500.00', @validFrom),	
	-- Export Disposal IsInterim 0 Range 101-500
	 ('FADF8A97-12FE-4BCA-94CD-AFD286B702F2', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '11600.00', '8600.00', @validFrom),
	-- Export Disposal IsInterim 0 Range 501-
	 ('1AA07E04-7267-4EE0-A006-4DE08B45B79B', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '12AF7EA4-1E60-4D35-B965-A4C9010901C7', '20500.00', '17500.00', @validFrom),

	-- Export Disposal IsInterim 1 Range 1-1
	 ('2101E4C5-047C-4517-A627-C06B279EB1AC', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '3000.00', '0.00', @validFrom),
	-- Export Disposal IsInterim 1 Range 2-5
	 ('4B53EAA2-1F05-4242-BB1B-8C64AD0C2CE3', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '3200.00', '0.00', @validFrom),
	-- Export Disposal IsInterim 1 Range 6-20
	 ('A0930CB3-1952-48C3-A009-15042154EDA0', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '4330.00', '1130.00', @validFrom),
	-- Export Disposal IsInterim 1 Range 21-100
	 ('CD19A0F7-1E29-4C9E-B5B4-472A60481564', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '7000.00', '3800.00', @validFrom),	
	-- Export Disposal IsInterim 1 Range 101-500
	 ('DC3F9A89-0A50-4FC7-81CE-608C29995E28', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '13900.00', '10700.00', @validFrom),
	-- Export Disposal IsInterim 1 Range 501-
	 ('A73869D4-B36F-424F-84E5-FD9715C66F47', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', '8385CAD7-E5F0-4765-A46B-A4C9010901C7', '25000.00', '21800.00', @validFrom),

	 
	 -- Import Recovery IsInterim 0 Range 1-1
	 ('D634AEEA-AAB5-422D-AB13-54A97C43A92C', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '2250.00', '0.00', @validFrom),
	-- Import Recovery IsInterim 0 Range 2-5
	 ('83B03F84-49AD-494A-AE55-6BB24E4B13A9', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '2450.00', '0.00', @validFrom),
	-- Import Recovery IsInterim 0 Range 6-20
	 ('72C46658-5F46-4183-8B22-7815A19D5170', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '3700.00', '1250.00', @validFrom),
	-- Import Recovery IsInterim 0 Range 21-100
	 ('8D2A9145-6DA2-4EC5-8E5C-4186D2FA21B4', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '5900.00', '3450.00', @validFrom),	
	-- Import Recovery IsInterim 0 Range 101-500
	 ('839B7B84-A0AB-4933-A3FA-E179F1281302', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '11600.00', '9150.00', @validFrom),
	-- Import Recovery IsInterim 0 Range 501-
	 ('D69EEA14-1781-4769-8B0A-9E5BBA6B69E7', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'F0407E39-C9BA-4519-B659-A4C9010901C7', '20500.00', '18050.00', @validFrom),

	-- Import Recovery IsInterim 1 Range 1-5
	 ('F3FEE9C7-0F0C-4440-B6AA-DF3F258442E5', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '2450.00', '0.00', @validFrom),
	-- Import Recovery IsInterim 1 Range 2-5
	 ('58AAA300-B015-4AD4-B8C0-40C883A52B2E', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '2650.00', '0.00', @validFrom),
	-- Import Recovery IsInterim 1 Range 6-20
	 ('0D9A227B-D275-4429-A97C-9B1B14933A7E', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '3830.00', '1180.00', @validFrom),
	-- Import Recovery IsInterim 1 Range 21-100
	 ('D4DF130E-FB00-47D4-814D-07E932CD1AF1', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '6500.00', '3850.00', @validFrom),	
	-- Import Recovery IsInterim 1 Range 101-500
	 ('84D25343-3DEB-4132-925B-D422B980CCC3', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '13900.00', '11250.00', @validFrom),
	-- Import Recovery IsInterim 1 Range 501-
	 ('D47FE5B3-0F61-4073-BB76-842E7F31FAEF', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'DAF51836-41E0-4324-93AE-A4C9010901C7', '25000.00', '22350.00', @validFrom),
	 
	-- Import Disposal IsInterim 0 Range 1-2-5
	 ('D23169EA-811D-4771-9FC6-B7473625E005', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '2840.00', '0.00', @validFrom),
	-- Import Disposal IsInterim 0 Range 2-5
	 ('18B2A212-546E-46E8-98DD-8F777A67FC4F', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '3040.00', '0.00', @validFrom),
	-- Import Disposal IsInterim 0 Range 6-20
	 ('301B59AA-59C1-465D-A389-79EDC9ADE05F', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '4330.00', '1290.00', @validFrom),
	-- Import Disposal IsInterim 0 Range 21-100
	 ('015FC353-0A1B-45D0-B451-4B50263A182E', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '6500.00', '3460.00', @validFrom),	
	 -- Import Disposal IsInterim 0 Range 101-500
	 ('F29C68E9-367D-468A-B0C8-8F0BDA8CED01', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '11600.00', '8560.00', @validFrom),
	-- Import Disposal IsInterim 0 Range 501-
	 ('571F2C9E-2407-485F-B404-A948525D8D4C', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'E5D7D07A-1FC3-45AC-AE0F-A4C9010901C7', '20500.00', '17460.00', @validFrom),

	-- Import Disposal IsInterim 1 Range 1-5
	 ('C9D14415-C2F1-40B7-99DB-0FFD03F5B1C3', 2, 'C9A002C1-0791-431E-B165-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '3000.00', '0.00', @validFrom),
	-- Import Disposal IsInterim 1 Range 2-5
	 ('3BD47E4E-5459-45E1-97E5-DC4059ADEDC5', 2, 'E3BC18DD-EBE4-414A-9067-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '3200.00', '0.00', @validFrom),
	-- Import Disposal IsInterim 1 Range 6-20
	 ('81FF2240-AEED-4352-8D04-441630BF89D8', 2, '805CC74A-1D68-44B9-ADC3-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '4330.00', '1130.00', @validFrom),
	-- Import Disposal IsInterim 1 Range 21-100
	 ('5A042096-CEBE-4D75-BEAA-0787F91FDF5F', 2, '85270610-00B0-4C09-A1DB-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '7000.00', '3800.00', @validFrom),	
	 -- Import Disposal IsInterim 1 Range 101-500
	 ('83920DCF-FB88-43E7-90EA-7F7B040CA803', 2, '5FBA5475-EC27-4645-AAC5-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '13900.00', '10700.00', @validFrom),
	-- Import Disposal IsInterim 1 Range 501-
	 ('EB2AF540-74A3-452F-8921-A5E50D341F73', 2, 'AEF5F83D-E5FB-41B2-84D7-A4C90107D231', 'BE00F07B-41E1-4C03-9BB9-A4C9010901C7', '25000.00', '21800.00', @validFrom);