PRINT 'Moving table schema';
IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Notification' 
                 AND  TABLE_NAME = 'Exporter'))
BEGIN
ALTER SCHEMA Business
TRANSFER Notification.Exporter
END

IF (EXISTS (SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'Business' 
                 AND  TABLE_NAME = 'NotificationProducer'))
BEGIN
	ALTER SCHEMA Notification
	TRANSFER Business.NotificationProducer
END