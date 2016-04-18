DECLARE @id uniqueidentifier
DECLARE @orgid uniqueidentifier
DECLARE @getid CURSOR

SET @getid = CURSOR FOR
SELECT
  u.Id,
  u.OrganisationId
FROM [Identity].AspNetUsers u
LEFT OUTER JOIN [Identity].AspNetUserClaims c
  ON c.UserId = u.Id
WHERE NOT EXISTS (SELECT
  *
FROM [Identity].AspNetUserClaims
WHERE userid = u.Id
AND ClaimType = 'organisation_id')

OPEN @getid
FETCH NEXT
FROM @getid INTO @id, @orgid
WHILE @@FETCH_STATUS = 0
BEGIN
  IF @orgid IS NOT NULL
    INSERT INTO [Identity].[AspNetUserClaims] ([UserId]
    , [ClaimType]
    , [ClaimValue])
      VALUES (LOWER(convert(nvarchar(50), @id)), 'organisation_id', LOWER(convert(nvarchar(50), @orgid)))
  FETCH NEXT
  FROM @getid INTO @id, @orgid

END

CLOSE @getid
DEALLOCATE @getid