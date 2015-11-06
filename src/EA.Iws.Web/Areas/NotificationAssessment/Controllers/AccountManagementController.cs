namespace EA.Iws.Web.Areas.NotificationAssessment.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Core.NotificationAssessment;
    using ViewModels.AccountManagement;

    [Authorize(Roles = "internal")]
    public class AccountManagementController : Controller
    {
        [HttpGet]
        public ActionResult Index(Guid id)
        {
            var data = CreateTestData();
            var model = new AccountManagementViewModel(data);
            
            return View(model);
        }

        private AccountManagementData CreateTestData()
        {
            var amd = new AccountManagementData();

            amd.TotalBillable = 1856.34m;
            amd.Balance = 345.98m;

            var tableData = new List<PaymentHistoryTableData>();

            tableData.Add(new PaymentHistoryTableData
            {
                Transaction = 1,
                Date = new DateTime(2015, 8, 23),
                Amount = 200.33m,
                Type = 0,
                Receipt = "2345097 NJK",
                Comments = "This is a comment"
            });

            tableData.Add(new PaymentHistoryTableData
            {
                Transaction = 1,
                Date = new DateTime(2015, 9, 15),
                Amount = 1000m,
                Type = 1,
                Receipt = "982734 NJK",
                Comments = string.Empty
            });

            tableData.Add(new PaymentHistoryTableData
            {
                Transaction = 1,
                Date = new DateTime(2015, 10, 12),
                Amount = 29.99m,
                Type = 2
            });

            tableData.Add(new PaymentHistoryTableData
            {
                Transaction = 2,
                Date = new DateTime(2015, 11, 5),
                Amount = 34.33m,
                Receipt = "EA 988969876",
                Comments = "This is another comment"
            });

            amd.PaymentHistory = tableData;

            return amd;
        }
    }
}