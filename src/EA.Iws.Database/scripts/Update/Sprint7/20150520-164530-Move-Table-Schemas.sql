PRINT 'Moving table schema';
ALTER SCHEMA Business
TRANSFER Notification.Exporter

ALTER SCHEMA Notification
TRANSFER Business.NotificationProducer
GO