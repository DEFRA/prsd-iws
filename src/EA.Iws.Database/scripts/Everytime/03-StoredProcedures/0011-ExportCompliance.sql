IF OBJECT_ID('[Reports].[ExportCompliance]') IS NULL
    EXEC('CREATE PROCEDURE [Reports].[ExportCompliance] AS SET NOCOUNT ON;')
GO

ALTER PROCEDURE [Reports].[ExportCompliance]
  @DateType NVARCHAR(30)
 ,@CompetentAuthority INT
 ,@From DATE
 ,@To DATE

AS
BEGIN
SET NOCOUNT ON;

	DECLARE @ExportNotifications TABLE
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
		IntendedQuantity				DECIMAL(18,4),
		IntendedQuantityUnit			INT,
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
		FacilityName					NVARCHAR(MAX),
		WasteType						NVARCHAR(64)
	)

	DECLARE @ExportPreNoteProcess TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		MovementId				UNIQUEIDENTIFIER NOT NULL,
		HasNoPrenotification    BIT,
		WorkingDays				INT,
		CalendarDays			INT,
		PendingIncomplete		INT
	)

	DECLARE @ExportPreNote TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		PrenotificationCount    INT
	)

	DECLARE @ShipmentQuantityConverter TABLE
	(
		FromUnit			INT NOT NULL,   
		ToUnit				INT NOT NULL,  
		ConversionRate      decimal(18,4) not null
		,PRIMARY KEY(FromUnit, ToUnit)
	)

	INSERT INTO @ShipmentQuantityConverter values(1,3,1000)  -- T->K
	INSERT INTO @ShipmentQuantityConverter values(3,1,0.001) -- K->T
	INSERT INTO @ShipmentQuantityConverter values(2,4,1000)	 -- CM->L 
	INSERT INTO @ShipmentQuantityConverter values(4,2,0.001) -- L->CM

  --select * from [Lookup].[ShipmentQuantityUnit]
	DECLARE @ExportQuantityProcess TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		ReceivedQuantity		decimal(18,4),
		ReceivedQuantityUnit	int,
		NotificationShipmentUnit	int,
		ConvertedQuantity		decimal(18,4)
	)

	DECLARE @ExportQuantity TABLE
	(
		NotificationId			UNIQUEIDENTIFIER NOT NULL,   
		ReceivedQuantity		decimal(18,4)
	)


	INSERT INTO @ExportNotifications 
SELECT DISTINCT A.*	
		,CASE
			WHEN WCI_EWC.IsNotApplicable = 1 THEN 'Not applicable'
			ELSE STUFF(( SELECT ', ' + WC.Code AS [text()]
					FROM [Notification].[WasteCodeInfo] WCI
					LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
					WHERE WCI.NotificationId = A.Id AND WC.CodeType = 3
					order by 1
					FOR XML PATH('')
					), 1, 1, '' )
		END AS [EwcCode],
		CASE
			WHEN WCI_YCODE.IsNotApplicable = 1 THEN 'Not applicable'
			ELSE STUFF(( SELECT ', ' + WC.Code AS [text()]
					FROM [Notification].[WasteCodeInfo] WCI
					LEFT JOIN [Lookup].[WasteCode] WC ON WCI.WasteCodeId = WC.Id
					WHERE WCI.NotificationId = A.Id AND WC.CodeType = 4
					order by 1
					FOR XML PATH('')
					), 1, 1, '' ) 
		END AS [YCode],
		SE_EEP.[Name] AS [PointOfExit],
        SI_EEP.[Name] AS [PointOfEntry],
		SE_C.[Name] AS [ExportCountryName],
        SI_C.[Name] AS [ImportCountryName],
		CASE
			WHEN SiteOfExport.[Id] IS NOT NULL THEN SiteOfExport.[Name]
			ELSE ''
		END AS [SiteOfExportName],
		STUFF(( SELECT ', ' + F.[Name] AS [text()]
					FROM [Notification].[Facility] F
					INNER JOIN [Notification].[FacilityCollection] FC ON F.FacilityCollectionId = FC.Id
					WHERE FC.NotificationId = A.Id
					order by 1
					FOR XML PATH('')
					), 1, 1, '' ) AS [FacilityName],
			CCT.[Description] AS [WasteType]
FROM
	(SELECT DISTINCT
        N.Id,
		REPLACE(N.NotificationNumber, ' ', '') AS NotificationNumber,
		N.CompetentAuthority AS CompetentAuthorityId,
		E.[Name] AS [NotifierName],
		I.[Name] AS [ConsigneeName],
		CASE When C.[To] <= GETDATE() THEN 'Y' ELSE 'N' End AS [FileExpired] ,
		COUNT(m.id) AS [NoOfShipment]
		,MAX(M.Number) AS [LatestShipmentNo]
		,(SELECT COUNT(M1.Id) FROM [Notification].[Movement] M1
			WHERE M1.[NotificationId] = N.Id
			AND M1.[Status] IN (1, 2, 3, 7)
			AND M1.[Date] <= GETDATE()) as [NoOfActualShipment]
		,FG.[ActiveLoadsPermitted]
		,S.Quantity
		,S.[Units]
		,S.NumberOfShipments
		,C.[From] AS [ConsentFrom],
        C.[To] AS [ConsentTo],
		D.[NotificationReceivedDate] AS [NotificationReceivedDate]	
		FROM [Notification].[Notification] AS N
		INNER JOIN	[Notification].[NotificationAssessment] AS NA 
		 ON		[NA].[NotificationApplicationId] = [N].[Id]
			AND NA.[Status]  = 10 --CONSENTED
		INNER JOIN  [Notification].[FinancialGuaranteeCollection] FGC ON FGC.[NotificationId] = N.Id
		INNER JOIN [Notification].[FinancialGuarantee] FG ON FG.Id = 
		(SELECT TOP 1 FG1.Id from [Notification].[FinancialGuarantee] FG1 
		 WHERE FG1.FinancialGuaranteeCollectionId = FGC.Id AND FG1.[Status] = 4 --Approved
		 ORDER BY FG1.CreatedDate DESC)	
		INNER JOIN 
		[Notification].[Movement] AS M
			ON M.NotificationId = N.Id  
		INNER JOIN [Notification].[Exporter] E ON E.NotificationId = N.Id
		INNER JOIN [Notification].[Importer] I ON I.NotificationId = N.Id
		INNER JOIN	[Notification].[Consent] AS C
		ON	[N].[Id] = [C].[NotificationApplicationId]
		INNER JOIN [Notification].[ShipmentInfo] S ON S.NotificationId = N.Id	
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
		,FG.[ActiveLoadsPermitted]
		,S.Quantity
		,S.[Units]
		,S.NumberOfShipments
		,C.[From]
		,C.[To]
		,D.[NotificationReceivedDate]
		) AS A
		LEFT JOIN [Notification].[Producer] SiteOfExport
        ON SiteOfExport.Id = 
        (
            SELECT TOP 1 P1.Id

            FROM		[Notification].[ProducerCollection] AS PC

            INNER JOIN [Notification].[Producer] AS P1
            ON		   PC.Id = P1.ProducerCollectionId

            WHERE		PC.NotificationId = A.Id 
						AND [IsSiteOfExport] = 1
            ORDER BY	P1.[IsSiteOfExport] DESC
        )
		INNER JOIN [Notification].[WasteCodeInfo] WCI_EWC ON WCI_EWC.NotificationId = A.Id AND WCI_EWC.CodeType = 3
		INNER JOIN [Notification].[WasteCodeInfo] WCI_YCODE ON WCI_YCODE.NotificationId = A.Id AND WCI_YCODE.CodeType = 4
		INNER JOIN [Notification].[TransportRoute] TR ON TR.NotificationId = A.Id
		INNER JOIN [Notification].[StateOfExport] SE ON SE.TransportRouteId = TR.Id
		INNER JOIN [Notification].[StateOfImport] SI ON SI.TransportRouteId = TR.Id
		INNER JOIN [Notification].[EntryOrExitPoint] SE_EEP ON SE_EEP.Id = SE.ExitPointId
		INNER JOIN [Notification].[EntryOrExitPoint] SI_EEP ON SI_EEP.Id = SI.EntryPointId
		INNER JOIN [Lookup].[Country] SI_C ON SI_C.Id = SI.CountryId
		INNER JOIN [Lookup].[Country] SE_C ON SE_C.Id = SE.CountryId
		INNER JOIN [Notification].[WasteType] WT1 ON WT1.NotificationId = A.Id
		INNER JOIN [Lookup].[ChemicalCompositionType] CCT ON CCT.Id = WT1.ChemicalCompositionType		

/* Number of shipments with No Pre-notification - This is the subtotal number of shipments with
  ‘No prenotification received',PLUS the total number of shipments that the difference between the pre-notification date 
  and the actual date of shipment is less than 3 working days or greater than 30 calendar days.*/
	INSERT INTO @ExportPreNoteProcess
	SELECT 
		N.NotificationId,
		M.ID,
		CASE WHEN M.HasNoPrenotification = 1 THEN 1 ELSE 0 END,
		(DATEDIFF(DD, M.[PrenotificationDate],M.[Date]) 
	   - (DATEDIFF(WK, M.[PrenotificationDate],M.[Date]) * 2) 
	   - CASE WHEN DATEPART(DW, M.[PrenotificationDate]) = 1 THEN 1 ELSE 0 END 
	   + CASE WHEN DATEPART(DW,M.[Date]) = 1 THEN 1 ELSE 0 END 
	   -(SELECT COUNT(*) FROM [Lookup].[BankHoliday] AS h 
			WHERE h.CompetentAuthority = @competentAuthority AND h.[Date] BETWEEN M.[PrenotificationDate] AND M.[Date])
		),
		DATEDIFF(DD, M.[PrenotificationDate],M.[Date]) 
		 ,(SELECT CASE WHEN M1.PrenotificationDate IS NULL THEN 1 ELSE 0 END 
		FROM [Notification].[Movement] M1 
		WHERE M1.Id = M.id and ISNULL(M1.HasNoPrenotification, 0) = 0 and M1.[Status] in (1,7,3,4) )
	FROM
	@ExportNotifications N
	INNER JOIN [Notification].[Movement] AS M
	ON	M.NotificationId = N.NotificationId
	


	INSERT INTO @ExportPreNote
	SELECT P.NotificationId
	,SUM(CASE WHEN P.HasNoPrenotification = 1 THEN 1 ELSE 0 END) 
		+ SUM(CASE WHEN P.WorkingDays < 4 THEN 1 
				   WHEN P.CalendarDays > 30 THEN 1 ELSE 0 END) 
		+ SUM(CASE WHEN P.PendingIncomplete = 1 THEN 1 ELSE 0 END) as NoPreNote
	FROM @ExportPreNoteProcess P
	GROUP BY P.NotificationId


/*Number of active shipments over the limit - This is the calculation of the ‘Current active loads’  
	minus the ‘Active loads permitted’  as shown on the shipment summary page of each individual notification number.
 If the result is negative, then display as zero*/
	INSERT INTO @ExportQuantityProcess (NotificationId,ReceivedQuantity, ReceivedQuantityUnit, NotificationShipmentUnit)
	SELECT 
		N.NotificationId,
		SUM(MR.Quantity),
		MR.[Unit],
		N.IntendedQuantityUnit
		FROM
		@ExportNotifications N
		INNER JOIN		
	 		[Notification].[Movement] AS M
			ON M.NotificationId = N.NotificationId
		INNER JOIN	[Lookup].[MovementStatus] AS MS 
		    ON	[M].[Status] = [MS].[Id]
			AND [M].[Status] in (3,4) -- Received & Completed
		INNER JOIN	
		[Notification].[MovementReceipt] AS MR
		ON	[M].[Id] = [MR].[MovementId]
	GROUP BY 
	N.NotificationId,
	MR.[Unit],
	N.IntendedQuantityUnit

	--PENDING Shipment Unit conversion Get the ones with mismatch unit and convert
	
	Update EQ 
	set ConvertedQuantity = EQ.ReceivedQuantity * SQ.ConversionRate
	FROM @ExportQuantityProcess EQ
	JOIN @ShipmentQuantityConverter SQ ON
	SQ.FromUnit = EQ.ReceivedQuantityUnit AND
	SQ.ToUnit = EQ.NotificationShipmentUnit
	Where  EQ.ReceivedQuantityUnit <> EQ.NotificationShipmentUnit


	INSERT INTO @ExportQuantity
	SELECT A.NotificationId, 
	SUM(CASE WHEN A.ReceivedQuantityUnit = NotificationShipmentUnit THEN A.ReceivedQuantity ELSE A.ConvertedQuantity END)
	FROM @ExportQuantityProcess A
	GROUP BY A.NotificationId


	--G Green R RED A AMBER

	SELECT A.NotificationId, A.NotificationNumber, A.CompetentAuthorityId, A.PrenotificationCount,
	CASE WHEN A.PrenotificationCount = 0 THEN 'G'
		 WHEN A.PrenotificationCount <= 10 THEN 'A'
		 ELSE 'R' end
	,A.MissingShipments
	,CASE WHEN A.MissingShipments = 0 THEN 'G' 
			WHEN A.MissingShipments <= 10 THEN 'A'
		 ELSE 'R' end
	,CASE WHEN A.OverLimit < 0 THEN CAST(0 as NVARCHAR) else CAST(A.OverLimit as NVARCHAR) end 
	,CASE WHEN A.OverLimit <= 0 THEN 'G'
		  WHEN A.OverLimit <= 5 THEN 'A'  ELSE 'R' end
	,A.Overtonnage
	,CASE WHEN A.Overtonnage = 'Y' THEN 'R'  ELSE 'G' end
	,A.OverShipments
	,CASE WHEN A.OverShipments = 'Y' THEN 'R'  ELSE 'G' end
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
	,A.WasteType		
	FROM
	(
		SELECT N.NotificationId, N.NotificationNumber
		,N.CompetentAuthorityId
		,ISNULL(P.PrenotificationCount, 0) AS PrenotificationCount
		,ISNULL(N.LatestShipmentNo - N.NoOfShipmentsUsed, 0) as MissingShipments
		,N.CurrentActiveLoads - N.PermittedActiveLoads as OverLimit
		,CASE When N.IntendedQuantity - Q.ReceivedQuantity < 0 THEN 'Y' ELSE 'N' END as Overtonnage
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
		,N.WasteType		
		FROM
		@ExportNotifications N
		LEFT JOIN	
		@ExportPreNote P ON
		P.NotificationId = N.NotificationId
		LEFT JOIN
		@ExportQuantity Q ON
		Q.NotificationId = N.NotificationId
	) A
	ORDER BY A.NotificationNumber
END