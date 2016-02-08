namespace EA.Iws.Requests.Notification
{
    using System;
    using Core.Notification;

    public class WhatToDoNextData
    {
        public Guid Id { get; set; }

        public UKCompetentAuthority CompetentAuthority { get; set; }
    }
}
