  
DECLARE @baselcodetype INT;
SET @baselcodetype = (SELECT Id FROM [Lookup].[CodeType] WHERE [Name] = 'Basel');

DECLARE @ycodetype INT;
SET @ycodetype = (SELECT Id FROM [Lookup].[CodeType] WHERE [Name] = 'Y-Code');

INSERT INTO [Lookup].[WasteCode] (Id,Code,[Description],CodeType,Active)
values (
'F566C8A3-0E60-4E5C-9BC5-7A34DC11DD07'
,'A1181'
,'Waste electrical and electronic equipment - containing or contaminated with cadmium, lead, mercury, 
  organohalogen compounds or other Annex I constituents to an extent that the waste exhibits an Annex III characteristic, 
  or - with a component containing or contaminated with Annex I constituents to an extent that the component exhibits an Annex III characteristic, 
  including but not limited to any of the following components: glass from cathode-ray tubes included on list A; a battery included on list A; a switch, lamp, 
  fluorescent tube or a display device backlight which contains mercury; a capacitor containing PCBs; a component containing asbestos; certain circuit boards; 
  certain display devices; certain plastic components containing a brominated flame retardant. Waste components of electrical and electronic 
  equipment containing or contaminated with Annex I constituents to an extent that the waste components exhibit an Annex III characteristic, 
  unless covered by another entry on list A. Wastes arising from the processing of waste electrical and electronic equipment or waste components of electrical and 
  electronic equipment, and containing or contaminated with Annex I constituents to an extent that the waste exhibits an Annex III characteristic 
  (e.g. fractions arising from shredding or dismantling), unless covered by another entry on list A'
,@baselcodetype
,'1')

INSERT INTO [Lookup].[WasteCode]([Id],[Code],[Description],[CodeType],Active) 
VALUES (
'21FD3CF3-0867-42C8-AEC0-6E910C95E623',
'Y49',
'Waste electrical and electronic equipment - not containing and not contaminated with Annex I constituents to an extent that the waste exhibits an Annex III characteristic, 
 and - in which none of the components (e.g. certain circuit boards, certain display devices) contain or are contaminated with Annex I constituents to an extent that the component 
 exhibits an Annex III characteristic. Waste components of electrical and electronic equipment (e.g. certain circuit boards, certain display devices) not containing 
 and not contaminated with Annex I constituents to an extent that the waste components exhibit an Annex III characteristic, unless covered by another entry in Annex II 
 or by an entry in Annex IX. Wastes arising from the processing of waste electrical and electronic equipment or waste components of electrical and electronic equipment 
 (e.g. fractions arising from shredding or dismantling), and not containing and not contaminated with Annex I constituents to an extent that the waste exhibits an Annex III characteristic,
 unless covered by another entry in Annex II or by an entry in Annex IX', 
@ycodetype,
'1');

ALTER TABLE [Notification].[WasteCodeInfo] ALTER COLUMN CustomCode NVARCHAR(1000);