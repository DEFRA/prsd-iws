PRINT 'Adding column to organisation table';

ALTER TABLE Business.Organisation
ADD OtherDescription NVARCHAR(2048);
GO