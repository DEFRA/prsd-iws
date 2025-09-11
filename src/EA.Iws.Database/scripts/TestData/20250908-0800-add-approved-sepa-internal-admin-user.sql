-- Inserting SEPA admin account

INSERT INTO [Identity].[AspNetUsers]
(
       Id,
       Email, 
       EmailConfirmed,
       PasswordHash,
       SecurityStamp,
       PhoneNumberConfirmed,
       TwoFactorEnabled,
       LockoutEnabled,
       AccessFailedCount,
       UserName,
       FirstName,
       Surname
)
VALUES
(
       'E7D9AB70-84FC-45C3-99A7-B437D1336402',
       'sepaadmin@environment-agency.gov.uk', 
       1, 
       'ANubTHWcySaAyFA68WgviJdTq0KOF4/iv5MTPvVPwRgmo0+CuzLTEszU0BcUYtOOTA==', 
       '9a241d0d-26d7-4783-a4a5-4ed3f4446a6b',
       0,
       0,
       1,
       0,
       'sepaadmin@environment-agency.gov.uk',
       'Sepa',
       'Admin'
);

GO

INSERT INTO [Person].[InternalUser]
(
    [Id],
    [UserId],
    [JobTitle],
    [CompetentAuthority],
    [LocalAreaId],
    [Status]
)
VALUES
(
    NEWID(),
    'E7D9AB70-84FC-45C3-99A7-B437D1336402',
    'Sepa Administrator',
    2,
    (SELECT TOP 1 [Id] FROM [Lookup].[LocalArea] WHERE Name = 'Head Office'),
    1
);
