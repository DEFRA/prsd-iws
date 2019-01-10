
USE [EA.Iws]
GO

INSERT INTO [Lookup].[AuditType]
(Id, AuditType)
VALUES
(1,'Add'),
(2,'Update'),
(3,'Delete')

GO

INSERT INTO [Lookup].[Screen]
(Id, ScreenName)
VALUES
(1,'Exporter - notifier'),
(2,'Waste generator -producer'),
(3,'Site of export'),
(4,'Importer - consignee'),
(5,'Recovery facility AND Disposal facility (notification type dependant)'),
(6,'Actual site of recovery AND Actual site of disposal (notification type dependant)'),
(7,'Preconsent facility (Recovery notification type only)'),
(8,'Recovery codes'),
(9,'Technology employed'),
(10,'Reason for export'),
(11,'Intended carrier'),
(12,'Means of transport'),
(13,'Packaging types'),
(14,'Special handling'),
(15,'Transport route export'),
(16,'Transport route import'),
(17,'Transport route transits'),
(18,'Customs office'),
(19,'Intended shipments'),
(20,'Chemical composition'),
(21,'Process of generation'),
(22,'Physical characteristics'),
(23,'Basel or OECD codes'),
(24,'EWC codes'),
(25,'Y codes'),
(26,'H or HP codes'),
(27,'UN classes'),
(28,'UN numbers'),
(29,'Other codes'),
(30,'Recovery information (Recovery notification type only)')