namespace EA.Iws.Web.Mappings
{
    using Areas.NotificationApplication.ViewModels.Submit;
    using Core.Notification;
    using Prsd.Core.Mapper;
    using System;
    using Prsd.Core.Helpers;

    public class SubmitSideBarMap : IMapWithParentObjectId<SubmitSummaryData, SubmitSideBarViewModel>
    {
        public SubmitSideBarViewModel Map(SubmitSummaryData source, Guid parentId)
        {
            return new SubmitSideBarViewModel
            {
                Charge = source.Charge,
                CompetentAuthorityName = EnumHelper.GetDisplayName(source.CompetentAuthority),
                CreatedDate = source.CreatedDate,
                NotificationId = parentId,
                NotificationNumber = source.NotificationNumber,
                Status = source.Status
            };
        }
    }
}