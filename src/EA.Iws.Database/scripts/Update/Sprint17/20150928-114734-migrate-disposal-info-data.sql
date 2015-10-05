GO

INSERT INTO [Notification].DisposalInfo
([Id], [NotificationId], [Unit], [Amount], [Method])

SELECT 
(select CAST(CAST(NEWID() AS BINARY(10)) + CAST(GETDATE() AS BINARY(6)) AS UNIQUEIDENTIFIER)) as [Id]
	,[NotificationId]
      ,[DisposalUnit]
      ,[DisposalAmount]
	  ,LEFT(B.[MethodOfDisposal], 40)
  FROM [Notification].[RecoveryInfo] A
  INNER JOIN 
  [Notification].[Notification]  B on A.NotificationId = B.Id
  WHERE A.[DisposalUnit]  IS NOT NULL
   
GO