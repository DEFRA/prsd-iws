DECLARE @CodeType INT = 3; --3 =EWC

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('C8A70232-9A58-4238-ACEF-0B5E7C666B1E',
																				'09 01 11* (EU)',
																				'single-use cameras containing batteries included in 16 06 01 to 16 06 04, 16 06 07 to 16 06 11 or 16 06 14',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('C7DAC16F-99DF-4EEF-91DC-1DF0728811CB',
																				'10 08 21* (EU)',
																				'slags from waste lithium-based battery recycling containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('19CD4EB2-3C7B-478D-AF2C-655A3199274E',
																				'10 08 22 (EU)',
																				'slags from waste lithium-based battery recycling other than those mentioned in 10 08 21',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('A867ACFB-350A-4782-88A1-C36F56A93B87',
																				'10 08 23* (EU)',
																				'slags from waste nickel-based battery recycling containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('6B643101-D902-49C8-9391-ACF382B4CAEB',
																				'10 08 24 (EU)',
																				'slags from waste nickel-based battery recycling other than those mentioned in 10 08 23',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('657D4F0A-A74F-4496-8CA9-1356CA3BC228',
																				'10 08 25* (EU)',
																				'slags from other waste battery recycling containing hazardous substances except 10 04 01, 10 08 21 and 10 08 23',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('611CEACB-F41A-4438-97D1-5F4951AA0628',
																				'10 08 26 (EU)',
																				'slags from other waste battery recycling other than those mentioned in 10 08 25',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('85D61540-6DB1-4161-8FD1-EC6B4F7D62CD',
																				'16 06 01* (EU)',
																				'waste lead-acid batteries',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('2E8B7254-D28F-4465-8D36-78135B79FD42',
																				'16 06 04* (EU)',
																				'waste alkaline-based batteries (other than those mentioned in 16 06 03)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('7075DE31-5333-4BA2-9FA2-132A262D0674',
																				'16 06 06* (EU)',
																				'separately collected electrolyte from waste batteries',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('99EC31D4-EFF4-4FFB-A7AF-4B361749A8FA',
																				'16 06 07* (EU)',
																				'waste lithium-based batteries',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('57E0C2AB-DAAC-4E92-9911-93A58147C664',
																				'16 06 08* (EU)',
																				'waste nickel-based batteries other than those mentioned in 16 06 02 (for example NiMH, Na-NiCl2)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('D7BF9C01-F255-473C-955C-568123EE0CA7',
																				'16 06 09* (EU)',
																				'waste zinc-based batteries, including silver oxide batteries',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('3942E87E-F302-49B2-815C-2EB399507C1E',
																				'16 06 10* (EU)',
																				'waste sodium-based batteries containing hazardous substances (except 16 06 11)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('3D5196E3-1D0B-4BB6-87CD-48A295A8E21D',
																				'16 06 11* (EU)',
																				'waste sodium sulphur batteries',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('7A0FB1BF-8BBE-4849-AE8D-5ED6AD96EC8E',
																				'16 06 12 (EU)',
																				'other waste sodium-based batteries (except 16 06 10 and 16 06 11)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('5D4F66EC-5A0C-4C46-90BE-6A98F5BC68E5',
																				'16 06 13* (EU)',
																				'mixed waste batteries',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('16503692-2DA0-4346-9595-EC17D90C9B68',
																				'16 06 14* (EU)',
																				'other waste batteries containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('7CE27A27-8B43-4CD7-98E9-AAC08F3FF80E',
																				'16 06 15 (EU)',
																				'waste batteries not otherwise specified other than those mentioned in 16 06 12 and 16 06 14',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('BA07A09F-1E09-443D-B7E5-A4F5CFEA44B2',
																				'16 06 22* (EU)',
																				'lead-acid battery manufacturing waste containing hazardous substances (for example lead paste)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('F0AA09F6-005D-4F36-B5EC-D1F444797A95',
																				'16 06 23 (EU)',
																				'lead-acid battery manufacturing waste other than that mentioned in 16 06 22',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('F3F43D62-FAD8-4E15-BA6D-7407C4BB0FF8',
																				'16 06 24* (EU)',
																				'lithium-based battery manufacturing waste containing hazardous substances (for example cathode cut-offs, cathode slurry, off specification battery cells, modules and/or packs)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('60D1DE6D-922B-4599-8200-6F7E13C1F91A',
																				'16 06 25 (EU)',
																				'lithium-based battery manufacturing waste other than those mentioned in 16 06 24 (for example anode cut-offs)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('D086E293-C6A5-49A5-A9D6-0E17ED13CCA9',
																				'16 06 26* (EU)',
																				'nickel-based battery manufacturing waste containing hazardous substances (for example liquid and solid cathode material)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('9BC13AD3-D50A-4A12-B9F8-43CEE9791AE7',
																				'16 06 27 (EU)',
																				'nickel-based battery manufacturing waste other than that mentioned in 16 06 26',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('C6A9259D-5AD1-4F31-A7EC-5B8E595A5B50',
																				'16 06 28* (EU)',
																				'alkaline-based battery manufacturing waste containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('E474A866-6388-4468-8AED-29197B526A95',
																				'16 06 29 (EU)',
																				'alkaline-based battery manufacturing waste other than that mentioned in 16 06 28',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('A889D7B7-A732-420E-BD7D-3DE8D5F508B6',
																				'16 06 30* (EU)',
																				'zinc-based battery manufacturing waste containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('BFDF3A1C-F09E-442B-ADB1-D07E9FEEDDCD',
																				'16 06 31 (EU)',
																				'zinc-based battery manufacturing waste other than that mentioned in 16 06 30',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('3079802C-29DE-4B22-AA8C-E51FD5444253',
																				'16 06 32* (EU)',
																				'sodium-based battery manufacturing waste containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('CEAC9905-65E9-4D5D-9A00-09988BB76391',
																				'16 06 33 (EU)',
																				'sodium-based battery manufacturing waste other than that mentioned in 16 06 32',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('8FF9B83B-3F0E-4F3E-B62F-BF7AF6EA4FCB',
																				'16 06 34* (EU)',
																				'battery manufacturing waste containing hazardous substances other than that mentioned in 16 06 22, 16 06 24, 16 06 26, 16 06 28, 16 06 30 and 16 06 32',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('73368E96-84AC-44DC-8FAF-396AB8D7351A',
																				'16 06 35 (EU)',
																				'battery manufacturing waste other than that mentioned in 16 06 23, 16 06 25, 16 06 27, 16 06 29, 16 06 31 and 16 06 33',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('D0F2CCF3-2CA6-487E-897E-BEBDBA5EAFBD',
																				'19 02 12* (EU)',
																				'solid salts and solutions containing heavy metals from battery recycling',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('21BF9EDE-D510-441C-9F38-54ADE478B3A2',
																				'19 02 13* (EU)',
																				'other wastes containing hazardous substances',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('254A73AB-ED68-4064-B393-E05C0DF8CD48',
																				'19 14 01* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste lead-acid batteries and lead-acid battery manufacturing waste containing a mixture of electrode materials',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('40EAEBE8-80DD-47AA-ABB6-885CDB1CC0EF',
																				'19 14 02* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste lithium-based batteries and lithium-based battery manufacturing waste containing a mixture of electrode materials',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('9FC98449-43D2-4407-8231-FDF57DA91468',
																				'19 14 03* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste nickel-based batteries and nickel-based battery manufacturing waste containing a mixture of electrode materials',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('07A01958-7BE4-46DF-B19E-73A6C64F847E',
																				'19 14 04* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste alkaline-based batteries and alkaline-based battery manufacturing waste containing a mixture of electrode materials',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('4BF0FA2E-0544-4B27-82A7-34D18D1F2BE6',
																				'19 14 05* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste zinc-based batteries and zinc-based battery manufacturing waste containing a mixture of electrode materials',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('1B2E9653-D442-4E4A-975D-3F84E925494C',
																				'19 14 06* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste sodium-based batteries and sodium-based battery manufacturing waste containing a mixture of electrode materials',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('A0597669-5165-44D8-AC00-8A08A2C54744',
																				'19 14 07* (EU)',
																				'intermediate fraction from the thermal and/or mechanical treatment of waste batteries and battery manufacturing waste containing a mixture of electrode materials, not otherwise specified in 19 14 01 to 19 14 06',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('CFD2A339-CF84-45DF-906C-66C718706714',
																				'19 14 08 (EU)',
																				'alloys from waste battery recycling (in massive form)',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('9A751734-C91F-4805-B53E-8502C0B4E396',
																				'20 01 42* (EU)',
																				'waste batteries included in 16 06 01 to 16 06 04, 16 06 08 to 16 06 11 or 16 06 14 and mixed waste batteries containing those waste batteries including also 16 06 07',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('CDBCF017-892F-49D5-BC8D-3B39499B7B98',
																				'20 01 43* (EU)',
																				'waste lithium-based batteries included in 16 06 07',
																				@CodeType)

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType]) VALUES ('3E48EEFB-5C19-4DB5-B816-5E3BDA561264',
																				'20 01 44 (EU)',
																				'waste batteries other than those mentioned in 20 01 42 and 20 01 43',
																				@CodeType)
