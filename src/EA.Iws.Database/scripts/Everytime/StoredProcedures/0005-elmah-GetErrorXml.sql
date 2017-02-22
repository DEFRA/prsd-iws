/*
  
   ELMAH - Error Logging Modules and Handlers for ASP.NET
   Copyright (c) 2004-9 Atif Aziz. All rights reserved.
  
    Author(s):
  
        Atif Aziz, http://www.raboof.com
        Phil Haacked, http://haacked.com
  
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
  
      http://www.apache.org/licenses/LICENSE-2.0
  
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
  
*/

-- ELMAH DDL script for Microsoft SQL Server 2000 or later.

-- $Id: SQLServer.sql 776 2011-01-12 21:09:24Z azizatif $

IF OBJECT_ID('[Logging].[ELMAH_GetErrorXml]') IS NULL
    EXEC('CREATE PROCEDURE [Logging].[ELMAH_GetErrorXml] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Logging].[ELMAH_GetErrorXml]
(
    @Application NVARCHAR(60),
    @ErrorId UNIQUEIDENTIFIER
)
AS
BEGIN

    SET NOCOUNT ON

    SELECT 
        [AllXml]
    FROM 
        [Logging].[ELMAH_Error]
    WHERE
        [ErrorId] = @ErrorId
    AND
        [Application] = @Application

END
GO