-- Inserting SEPA admin account

DECLARE @UserId UNIQUEIDENTIFIER;
DECLARE @CompetentAuthority INT;

SET @UserId = NEWID();
SET @CompetentAuthority = 2; --2 = SEPA

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
       @UserId,
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
    @UserId,
    'Sepa Administrator',
    @CompetentAuthority,
    (SELECT [Id] FROM [Lookup].[LocalArea] WHERE Name = 'Head Office'),
    1
);
