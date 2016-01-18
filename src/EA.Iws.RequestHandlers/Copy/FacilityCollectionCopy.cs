namespace EA.Iws.RequestHandlers.Copy
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Core.ComponentRegistration;
    using DataAccess;
    using Domain.NotificationApplication;

    [AutoRegister]
    public class FacilityCollectionCopy
    {
        public async Task CopyAsync(IwsContext context, Guid notificationSourceId, Guid notificationDestinationId)
        {
            var originalFacilities = await context.Facilities
                .AsNoTracking()
                .Include("FacilitiesCollection")
                .SingleOrDefaultAsync(f => f.NotificationId == notificationSourceId);

            var newFacilities = new FacilityCollection(notificationDestinationId);

            if (originalFacilities != null)
            {
                foreach (var facility in originalFacilities.Facilities)
                {
                    var newFacility = newFacilities.AddFacility(facility.Business, facility.Address, facility.Contact);

                    if (facility.IsActualSiteOfTreatment)
                    {
                        typeof(Facility).GetProperty("IsActualSiteOfTreatment")
                            .SetValue(newFacility, true, null);
                    }
                }

                typeof(FacilityCollection).GetProperty("AllFacilitiesPreconsented")
                    .SetValue(newFacilities, originalFacilities.AllFacilitiesPreconsented, null);
            }

            context.Facilities.Add(newFacilities);
        }
    }
}