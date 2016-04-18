DECLARE @FirstName [nvarchar](256)
DECLARE @Surname [nvarchar](256)
DECLARE @id uniqueidentifier

DECLARE @getid CURSOR

SET @getid = CURSOR FOR
SELECT
  u.Id,
  u.FirstName,
  u.Surname
FROM [Identity].AspNetUsers u
LEFT OUTER JOIN [Identity].AspNetUserClaims c
  ON c.UserId = u.Id
WHERE NOT EXISTS (SELECT
  *
FROM [Identity].AspNetUserClaims
WHERE userid = u.Id
AND ClaimType = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name')

OPEN @getid
FETCH NEXT
FROM @getid INTO @id, @FirstName, @Surname
WHILE @@FETCH_STATUS = 0
BEGIN
  IF @FirstName IS NOT NULL
    INSERT INTO [Identity].[AspNetUserClaims] ([UserId]
    , [ClaimType]
    , [ClaimValue])
      VALUES (LOWER(convert(nvarchar(50), @id)), 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name', LOWER(convert(nvarchar(50), @FirstName + ' ' + @Surname)))
  FETCH NEXT
  FROM @getid INTO @id, @FirstName, @Surname

END

CLOSE @getid
DEALLOCATE @getid

