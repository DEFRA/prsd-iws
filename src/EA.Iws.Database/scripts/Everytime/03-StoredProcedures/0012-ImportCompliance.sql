IF OBJECT_ID('[Reports].[ImportCompliance]') IS NULL
    EXEC('CREATE PROCEDURE [Reports].[ImportCompliance] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Reports].[ImportCompliance]
  @DateType NVARCHAR(30)
 ,@CompetentAuthority INT
 ,@From DATE
 ,@To DATE
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @ImportNotifications TABLE
	(
		NotificationId					UNIQUEIDENTIFIER NOT NULL,       
		NotificationNumber				NVARCHAR(50)  NOT NULL,  
		CompetentAuthorityId			INT NOT NULL,     
		Notifier	                    NVARCHAR(255),       
		Consignee	                    NVARCHAR(255),      
		FileExpired	                    CHAR(1),
		NoOfShipmentsUsed				INT,
		LatestShipmentNo				INT,
		CurrentActiveLoads				INT,
		PermittedActiveLoads			INT,
		IntendedQuantity				decimal(18,4),
		IntendedQuantityUnit			int,
		IntendedNoOfShipments			INT,
		ConsentFrom						DATE,
		ConsentTo						DATE,
		NotificationReceivedDate		DATE,
		EWCCode							NVARCHAR(MAX),
		YCode							NVARCHAR(MAX),
		PointOfExit						NVARCHAR(2048),
        PointOfEntry					NVARCHAR(2048),
		ExportCountryName				NVARCHAR(2048),
        ImportCountryName				NVARCHAR(2048),
		SiteOfExportName				NVARCHAR(2048),
		FacilityName					NVARCHAR(MAX)
	)

	DECLARE @ImportQuantity TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		ReceivedQuantity		decimal(18,4)
	)

	DECLARE @ImportPreNote TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		PrenotificationCount    INT
	)

	DECLARE @ImportPreNoteProcess TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		MovementId				UNIQUEIDENTIFIER NOT NULL,
		HasNoPrenotification    BIT,
		DateDifference			INT   
	)

	INSERT INTO @ImportNotifications
	SELECT DISTINCT A.*,
	STUFF(( SELECT ', ' + WC.Code AS [text()]
            FROM [ImportNotification].[WasteType] WT
            INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
            INNER JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
            WHERE WT.ImportNotificationId = A.Id AND WC.CodeType = 3
            order by 1
            FOR XML PATH('')
            ), 1, 1, '' ) AS [EwcCode],
		CASE
			WHEN WT.YCodeNotApplicable = 1 THEN 'Not applicable'
			ELSE STUFF(( SELECT ', ' + WC.Code AS [text()]
					FROM [ImportNotification].[WasteType] WT
					INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId
					LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
					WHERE WT.ImportNotificationId = A.Id AND WC.CodeType = 4
					order by 1
					FOR XML PATH('')
					), 1, 1, '' )
		END AS [YCode],
		SE_EEP.[Name] AS [PointOfExit],
        SI_EEP.[Name] AS [PointOfEntry],
		SE_C.[Name] AS [ExportCountryName],
        SI_C.[Name] AS [ImportCountryName],
		P.[Name] [SiteOfExportName],
		STUFF(( SELECT ', ' + F.[Name] AS [text()]
			FROM [ImportNotification].[Facility] F
			INNER JOIN [ImportNotification].[FacilityCollection] FC ON F.FacilityCollectionId = FC.Id
			WHERE FC.ImportNotificationId = A.Id
			order by 1
			FOR XML PATH('')
			), 1, 1, '' ) AS [FacilityName]
FROM
(
	SELECT DISTINCT
        N.Id,
		REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
		N.CompetentAuthority AS CompetentAuthorityId,
		E.[Name]  AS [NotifierName],
		I.[Name] AS [ConsigneeName],
		CASE When C.[To] <= GETDATE() Then 'Y' Else 'N' End AS [FileExpired],
		COUNT(m.id) AS [NoOfShipment]
		,MAX(M.Number) AS [LatestShipmentNo]
		,NULL as [NoOfActualShipment]
		,NULL AS [ActiveLoadsPermitted]
		,S.Quantity
		,S.[Units]
		,S.NumberOfShipments,
		C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
		D.[NotificationReceivedDate] AS [NotificationReceivedDate]
		FROM [ImportNotification].[Notification] AS N
		INNER JOIN	[ImportNotification].[NotificationAssessment] AS NA 
		 ON		[NA].[NotificationApplicationId] = [N].[Id]
			AND NA.[Status]  = 9 --CONSENTED
		INNER JOIN 
		[ImportNotification].[Movement] AS M
			ON M.NotificationId = N.Id
		INNER JOIN [ImportNotification].[Exporter] E 
		INNER JOIN [Lookup].[Country] AS C_E ON E.[CountryId] = C_E.[Id]
		ON E.ImportNotificationId = N.Id
		INNER JOIN [ImportNotification].[Importer] I 
		INNER JOIN [Lookup].[Country] AS C_I ON I.[CountryId] = C_I.[Id]
		ON I.ImportNotificationId = N.Id
		INNER JOIN	[ImportNotification].[Consent] AS C
		ON	[N].[Id] = [C].NotificationId
		INNER JOIN [ImportNotification].Shipment S ON S.ImportNotificationId = N.Id	
		LEFT JOIN [Notification].NotificationDates D ON D.[NotificationAssessmentId] = NA.[Id]			 
		WHERE
		CompetentAuthority = @competentAuthority
	                AND (@dateType = 'NotificationReceivedDate' AND  [NotificationReceivedDate] BETWEEN @from AND @to
                                         OR @dateType = 'ConsentFrom' AND  C.[From]  BETWEEN @from AND @to
                                         OR @dateType = 'ConsentTo' AND  C.[To] BETWEEN @from AND @to	)	
		GROUP BY
		N.Id,
		N.NotificationNumber,
		N.CompetentAuthority,
		E.[Name],
		I.[Name],
		C.[To]		
		,S.Quantity
		,S.[Units]
		,S.NumberOfShipments
		,C.[From]
		,C.[To]
		,D.[NotificationReceivedDate]
	) AS A
		INNER JOIN [ImportNotification].[Producer] P ON P.ImportNotificationId = A.Id
	    INNER JOIN [ImportNotification].[TransportRoute] TR ON TR.ImportNotificationId = A.Id
		INNER JOIN [ImportNotification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
		INNER JOIN [ImportNotification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
		INNER JOIN [Notification].[EntryOrExitPoint] SE_EEP ON SE_EEP.Id = SE.ExitPointId
		INNER JOIN [Notification].[EntryOrExitPoint] SI_EEP ON SI_EEP.Id = SI.EntryPointId
		INNER JOIN [ImportNotification].[WasteType] WT ON WT.ImportNotificationId = A.Id
		INNER JOIN (
			SELECT TOP 1 [Id], [Name], [IsoAlpha2Code] 
			FROM [Lookup].[Country] 
			WHERE IsoAlpha2Code = 'GB' ) AS SI_C ON 1 = 1
		INNER JOIN [Lookup].[Country] SE_C ON SE_C.Id = SE.CountryId
		INNER JOIN [ImportNotification].[WasteCode] WCI ON WT.Id = WCI.WasteTypeId


	INSERT INTO @ImportPreNoteProcess
	SELECT 
		N.NotificationId,
		M.ID,
		CASE WHEN M.PrenotificationDate IS NULL THEN 1 ELSE 0 END,
		(DATEDIFF(DD, M.[PrenotificationDate],M.ActualShipmentDate) 
	   - (DATEDIFF(WK, M.[PrenotificationDate],M.ActualShipmentDate) * 2) 
	   - CASE WHEN DATEPART(DW, M.[PrenotificationDate]) = 1 THEN 1 ELSE 0 END 
	   + CASE WHEN DATEPART(DW,M.ActualShipmentDate) = 1 THEN 1 ELSE 0 END 
	   -(SELECT COUNT(*) FROM [Lookup].[BankHoliday] AS h 
			WHERE h.CreatedDate BETWEEN M.[PrenotificationDate] AND M.ActualShipmentDate)
		)
	FROM
	@ImportNotifications N
	INNER JOIN [ImportNotification].[Movement] AS M
	ON	M.NotificationId = N.NotificationId


	INSERT INTO @ImportPreNote
	SELECT P.NotificationId
	,SUM(case when P.HasNoPrenotification = 1 then 1 else 0 end) 
		+ SUM(case when P.DateDifference < 3 then 1 
				   when P.DateDifference > 30 then 1 else 0 end) as NoPreNote
	FROM @ImportPreNoteProcess P
	GROUP BY P.NotificationId



	INSERT INTO @ImportQuantity (NotificationId,ReceivedQuantity)
	SELECT 
		N.NotificationId,
		SUM(MR.Quantity)		
		FROM
		@ImportNotifications N
		INNER JOIN		
	 		[ImportNotification].[Movement] AS M
			ON M.NotificationId = N.NotificationId
		INNER JOIN	
		[ImportNotification].[MovementReceipt] AS MR
		ON	[M].[Id] = [MR].[MovementId]
	GROUP BY 
	N.NotificationId



	--G Green R RED A AMBER
	SELECT A.NotificationId, A.NotificationNumber, A.CompetentAuthorityId, A.PrenotificationCount,
	CASE WHEN A.PrenotificationCount = 0 then 'G'
		 WHEN A.PrenotificationCount <= 10 then 'A'
		 Else 'R' end
	,A.MissingShipments
	,CASE WHEN A.MissingShipments = 0 Then 'G' 
			WHEN A.MissingShipments <= 10 then 'A'
		 Else 'R' end
	,OverLimit
	,'N/A'
	,A.Overtonnage
	,CASE WHEN A.Overtonnage = 'Y' THEN 'R'  Else 'G' end
	,A.OverShipments
	,CASE WHEN A.OverShipments = 'Y' THEN 'R'  Else 'G' end
	,A.Notifier
	,A.Consignee
	,A.FileExpired
	,A.EWCCode					
	,A.YCode			
	,A.PointOfExit		
	,A.PointOfEntry			
	,A.ExportCountryName	
	,A.ImportCountryName		
	,A.SiteOfExportName	
	,A.FacilityName
	FROM
	(
		SELECT N.NotificationId, N.NotificationNumber
		,N.CompetentAuthorityId
		,P.PrenotificationCount
		,N.LatestShipmentNo - N.NoOfShipmentsUsed as MissingShipments
		,'N/A' as OverLimit
		,CASE When N.IntendedQuantity - Q.ReceivedQuantity < 0 Then 'Y' ELSE 'N' END as Overtonnage
		,CASE WHEN n.NoOfShipmentsUsed > N.IntendedNoOfShipments THEN 'Y' ELSE 'N' END AS OverShipments
		,N.Notifier
		,N.Consignee
		,N.FileExpired
		,N.EWCCode					
		,N.YCode			
		,N.PointOfExit		
		,N.PointOfEntry			
		,N.ExportCountryName	
		,N.ImportCountryName		
		,N.SiteOfExportName	
		,N.FacilityName	
		FROM
		@ImportNotifications N
		LEFT JOIN	
		@ImportPreNote P ON
		P.NotificationId = N.NotificationId
		LEFT JOIN
		@ImportQuantity Q ON
		Q.NotificationId = N.NotificationId
	) A
	ORDER BY A.NotificationNumber

END
GO