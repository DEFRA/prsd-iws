IF NOT EXISTS (SELECT * FROM dbo.sysusers WHERE NAME = 'iws_application') CREATE ROLE iws_application AUTHORIZATION db_securityadmin
GO