declare @BadNotifications table
(
	ImportNotificationId uniqueidentifier,
	ExporterId uniqueidentifier,
	ImporterId uniqueidentifier,
	ProducerId uniqueidentifier,
	FacilityCollectionId uniqueidentifier,
	ShipmentId uniqueidentifier,
	TransportRouteId uniqueidentifier,
	WasteOperationId uniqueidentifier,
	WasteTypeId uniqueidentifier
)

print N'
get bad notifications....'

insert into @BadNotifications
(
	ImportNotificationId
)
select ImportNotificationId 
from [ImportNotification].[Exporter] 
group by ImportNotificationId
having count(*) > 1 

print N'
get some entities to keep...'

update bn
set 
	ExporterId = e.ExporterId,
	ImporterId = i.ImporterId,
	ProducerId = p.ProducerId,
	ShipmentId = s.ShipmentId,
	WasteOperationId = wo.WasteOperationId,
	WasteTypeId = wt.WasteTypeId,
	FacilityCollectionId = fc.FacilityCollectionId,
	TransportRouteId = tr.TransportRouteId
from @BadNotifications bn
cross apply
	(select top 1 Id as ProducerId
	 from ImportNotification.Producer
	 where ImportNotificationId = bn.ImportNotificationId) as p
cross apply
	(select top 1 Id as ImporterId
	 from ImportNotification.Importer
	 where ImportNotificationId = bn.ImportNotificationId) as i
cross apply
	(select top 1 Id as ExporterId
	 from ImportNotification.Exporter
	 where ImportNotificationId = bn.ImportNotificationId) as e
cross apply
	(select top 1 Id as ShipmentId
	 from ImportNotification.Shipment
	 where ImportNotificationId = bn.ImportNotificationId) as s
cross apply
	(select top 1 Id as WasteOperationId
	 from ImportNotification.WasteOperation
	 where ImportNotificationId = bn.ImportNotificationId) as wo
cross apply
	(select top 1 Id as WasteTypeId
	 from ImportNotification.WasteType
	 where ImportNotificationId = bn.ImportNotificationId) as wt
cross apply
	(select top 1 Id as FacilityCollectionId
	 from ImportNotification.FacilityCollection
	 where ImportNotificationId = bn.ImportNotificationId) as fc
cross apply
	(select top 1 Id as TransportRouteId
	 from ImportNotification.TransportRoute
	 where ImportNotificationId = bn.ImportNotificationId) as tr

print N'
delete all except one for each entity type...'

delete e from [ImportNotification].[Exporter] e
inner join @BadNotifications bn
	on e.ImportNotificationId = bn.ImportNotificationId
where e.Id <> bn.ExporterId

delete i from [ImportNotification].[Importer] i
inner join @BadNotifications bn
	on i.ImportNotificationId = bn.ImportNotificationId
where i.Id <> bn.ImporterId

delete p from [ImportNotification].[Producer] p
inner join @BadNotifications bn
	on p.ImportNotificationId = bn.ImportNotificationId
where p.Id <> bn.ProducerId

delete s from [ImportNotification].[Shipment] s
inner join @BadNotifications bn
	on s.ImportNotificationId = bn.ImportNotificationId
where s.Id <> bn.ShipmentId

delete oc from [ImportNotification].[OperationCodes] oc
inner join [ImportNotification].[WasteOperation] wo
	on oc.WasteOperationId = wo.Id
inner join @BadNotifications bn
	on wo.ImportNotificationId = bn.ImportNotificationId
where wo.Id <> bn.WasteOperationId

delete wo from [ImportNotification].[WasteOperation] wo
inner join @BadNotifications bn
	on wo.ImportNotificationId = bn.ImportNotificationId
where wo.Id <> bn.WasteOperationId

delete wc from [ImportNotification].[WasteCode] wc
inner join [ImportNotification].[WasteType] wt
	on wc.WasteTypeId = wt.Id
inner join @BadNotifications bn
	on wt.ImportNotificationId = bn.ImportNotificationId
where wt.Id <> bn.WasteTypeId

delete wt from [ImportNotification].[WasteType] wt
inner join @BadNotifications bn
	on wt.ImportNotificationId = bn.ImportNotificationId
where wt.Id <> bn.WasteTypeId

delete f from [ImportNotification].[Facility] f
inner join [ImportNotification].[FacilityCollection] fc
	on f.FacilityCollectionId = fc.Id
inner join @BadNotifications bn
	on fc.ImportNotificationId = bn.ImportNotificationId
where fc.Id <> bn.FacilityCollectionId

delete fc from [ImportNotification].[FacilityCollection] fc
inner join @BadNotifications bn
	on fc.ImportNotificationId = bn.ImportNotificationId
where fc.Id <> bn.FacilityCollectionId

delete se from [ImportNotification].[StateOfExport] se
inner join [ImportNotification].[TransportRoute] tr
	on se.TransportRouteId = tr.Id
inner join @BadNotifications bn
	on tr.ImportNotificationId = bn.ImportNotificationId
where tr.Id <> bn.TransportRouteId

delete si from [ImportNotification].[StateOfImport] si
inner join [ImportNotification].[TransportRoute] tr
	on si.TransportRouteId = tr.Id
inner join @BadNotifications bn
	on tr.ImportNotificationId = bn.ImportNotificationId
where tr.Id <> bn.TransportRouteId

delete ts from [ImportNotification].[TransitState] ts
inner join [ImportNotification].[TransportRoute] tr
	on ts.TransportRouteId = tr.Id
inner join @BadNotifications bn
	on tr.ImportNotificationId = bn.ImportNotificationId
where tr.Id <> bn.TransportRouteId

delete tr from [ImportNotification].[TransportRoute] tr
inner join @BadNotifications bn
	on tr.ImportNotificationId = bn.ImportNotificationId
where tr.Id <> bn.TransportRouteId