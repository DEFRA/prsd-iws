-- This script adds a new column to the AspNetUsers table to store the last login date of the user.
ALTER TABLE [Identity].[AspNetUsers] ADD [LastLoginDate] DATETIME2 NULL;
GO