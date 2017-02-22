IF NOT EXISTS (SELECT [Name] FROM sys.synonyms WHERE [Name] = 'ELMAH_Error')
    CREATE SYNONYM [ELMAH_Error] FOR [Logging].[ELMAH_Error];

IF NOT EXISTS (SELECT [Name] FROM sys.synonyms WHERE [Name] = 'ELMAH_GetErrorsXml')
    CREATE SYNONYM [ELMAH_GetErrorsXml] FOR [Logging].[ELMAH_GetErrorsXml];

IF NOT EXISTS (SELECT [Name] FROM sys.synonyms WHERE [Name] = 'ELMAH_GetErrorXml')
    CREATE SYNONYM [ELMAH_GetErrorXml] FOR [Logging].[ELMAH_GetErrorXml];

IF NOT EXISTS (SELECT [Name] FROM sys.synonyms WHERE [Name] = 'ELMAH_LogError')
    CREATE SYNONYM [ELMAH_LogError] FOR [Logging].[ELMAH_LogError];