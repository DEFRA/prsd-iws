BEGIN
    SET NOCOUNT ON;
    DECLARE @CountryId          UNIQUEIDENTIFIER    = (SELECT NEWID()),
            @CountryName        NVARCHAR(2048)      = 'Ascension Island',
            @IsoAlpha2Code      NCHAR(2)            = 'AC',
            @IsEuropeanMember   bit                 = 0,
            @CAName             NVARCHAR(1023)      = 'Other',
            @IsSystemUser       bit                 = 0,
            @IsTransitAuthority bit                 = NULL,
            @CAcode             NVARCHAR(25);

    SET @CAcode = @IsoAlpha2Code; -- Currently set to be same as country code
    BEGIN TRY
        SET XACT_ABORT ON;
        BEGIN TRAN

            INSERT INTO [Lookup].[Country] ([Id],[Name],[IsoAlpha2Code],[IsEuropeanUnionMember])
            VALUES (@CountryId, @CountryName, @IsoAlpha2Code, @IsEuropeanMember);

            INSERT INTO [Lookup].[CompetentAuthority] ([Id],[Name],[Abbreviation],[IsSystemUser],[Code],[CountryId],[Region],[IsTransitAuthority])
            VALUES (NEWID(), @CAName, NULL, @IsSystemUser, @CAcode, @CountryId, NULL, @IsTransitAuthority);

        --COMMIT TRAN
        ROLLBACK TRAN

    END TRY
    BEGIN CATCH

        IF @@TRANCOUNT > 0
            ROLLBACK;
        THROW
    END CATCH
END