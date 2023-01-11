--IF OBJECT_ID('[Notification].[ArchiveNotification]') IS NULL
--    EXEC('CREATE PROCEDURE [Notification].[ArchiveNotification] AS SET NOCOUNT ON;')
--GO

--ALTER PROCEDURE [Notification].[ArchiveNotification]
--  @NotificationId UNIQUEIDENTIFIER
--AS
--BEGIN
--	SET NOCOUNT ON;
    
--	BEGIN TRAN

/*
✔Notifier contact name
✔Notifier contact email address
✔Notifier contact phone number
✔Notifier contact fax number

FullName, Email, Telephone, Fax FROM [Notification].[Exporter]		E ON N.Id = E.NotificationId
ContactName, Email, Telephone FROM [ImportNotification].[Exporter] E ON N.Id = E.ImportNotificationId

✔Producer(s) contact name
✔Producer(s) contact email address
✔Producer(s) contact phone number
✔Producer(s) contact fax number
FullName, Telephone, Email, Fax [Notification].[Producer] P ON PC.Id = P.ProducerCollectionId
ContactName, Telephone, Email [ImportNotification].[Producer] P ON N.Id = P.ImportNotificationId

✔Consignee contact name
✔Consignee contact email address
✔Consignee contact phone number
✔Consignee contact fax number
FullName, Telephone, Fax, Email [Notification].[Importer] I ON N.Id = I.NotificationId
ContactName, Telephone, Email [ImportNotification].[Importer] I ON N.Id = I.ImportNotificationId

✔Facility(s) contact name
✔Facility(s) contact email address
✔Facility(s) contact phone number
✔Facility(s) contact fax number        
Fullname, Telephone, Fax, Email	INNER JOIN [Notification].[FacilityCollection] FC ON N.Id = FC.NotificationId
        INNER JOIN [Notification].[Facility] F ON FC.Id = F.FacilityCollectionId		
ContactName, Telephone, Email LEFT JOIN [ImportNotification].[FacilityCollection] FC ON N.Id = FC.ImportNotificationId
        INNER JOIN [ImportNotification].[Facility] F ON FC.Id = F.FacilityCollectionId

Carrier(s) contact name
Carrier(s) contact email address
Carrier(s) contact phone number
Carrier(s) contact fax number

Recovery Facility details - Also needs to archive recovery details 
(Question for Sreedhar: Is this covered by the 'Facility' fields above or is this held in a separate table? Needs archiving regardless)
--This is in the Draft.Import table, may need to handle this special
Intended Carrier Contact*
Intended carrier phone number*
Intended carrier Email*
(Intended carrier details may be linked from other data fields (Shipment Details) so will ideally just automatically be archived when shipment details are archived. Sreedhar to confirm during dev - Archived data should be reflected in reports e.g. Shipment pre-notification document)

These data fields will ONLY be replaced with "Archived" IF company status of the (AND/OR) Notifier, Consignee, Producer(s),Facility(s) and Carrier(s) is labelled as ‘Sole Trader’ or ‘Partnership’:

External User name
Company name
Company address
*/