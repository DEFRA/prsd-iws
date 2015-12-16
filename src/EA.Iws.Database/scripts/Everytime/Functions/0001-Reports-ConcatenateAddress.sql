SET CONCAT_NULL_YIELDS_NULL ON;
GO

IF OBJECT_ID('[Reports].[ConcatenateAddress]') IS NULL
    EXEC('CREATE FUNCTION [Reports].[ConcatenateAddress]() RETURNS NVARCHAR(MAX) AS BEGIN RETURN '''' END;')
GO	

ALTER FUNCTION [Reports].[ConcatenateAddress](
			@addressLine1		NVARCHAR(1024), 
			@addressLine2		NVARCHAR(1024),
			@townOrCity			NVARCHAR(1024),
			@postalCode			NVARCHAR(64),
			@region				NVARCHAR(1024),
			@country			NVARCHAR(1024))
RETURNS NVARCHAR(MAX)
AS
BEGIN
	IF	@addressLine1 IS NULL
	AND	@addressLine2 IS NULL
	AND	@townOrCity	IS NULL	
	AND	@postalCode	IS NULL	
	AND	@region	IS NULL		
	AND	@country IS NULL
		RETURN NULL;		

	-- By concatenating using '+' if the variable is null the whole string becomes null.
	SET @addressLine1 = @addressLine1 + ',';
	SET @addressLine2 = ' ' + @addressLine2 + ',';
	SET @townOrCity = ' ' + @townOrCity + ',';
	SET @postalCode = ' ' + @postalCode + ',';
	SET @region = ' ' + @region + ',';
	SET @country = ' ' + @country;

	-- The Concat function will treat null as an empty string instead so we use it here to prevent the entire address string being null.
	RETURN CONCAT(@addressLine1, @addressLine2, @townOrCity, @postalCode, @region, @country);
END;
GO