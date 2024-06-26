﻿namespace EA.Iws.Core.Admin.Reports
{
    using System;
    using System.ComponentModel;

    public class FinancialGuaranteesData
    {
        public string ReferenceNumber { get; set; }

        public DateTime ApprovedDate { get; set; }

        public int ActiveLoadsPermitted { get; set; }

        public int CurrentActiveLoads { get; set; }

        public string NotificationNumber { get; set; }

        public string ExporterName { get; set; }

        public string ImporterName { get; set; }

        public string ProducerName { get; set; }

        [DisplayName("Blanket bond?")]
        public string IsBlanketBond { get; set; }

        [DisplayName("Amount of cover provided")]
        public decimal? CoverAmount { get; set; }
        
        [DisplayName("Calculation continued")]
        public decimal? CalculationContinued { get; set; }

        [DisplayName("Over active loads (Y/N)")]
        public string OverActiveLoads { get; set; }
    }
}