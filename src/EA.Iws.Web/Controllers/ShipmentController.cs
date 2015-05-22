namespace EA.Iws.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;
    using System.Web.Mvc;
    using Requests.Notification;
    using ViewModels.Shipment;

    [Authorize]
    public class ShipmentController : Controller
    {
        [HttpGet]
        public ActionResult Info(Guid id)
        {
            var model = new ShipmentInfoViewModel();
            model.NotificationId = id;

            IEnumerable<ShipmentQuantityUnits> shipmentQuantityUnits =
                Enum.GetValues(typeof(ShipmentQuantityUnits)).Cast<ShipmentQuantityUnits>();
            model.UnitsSelectList = from units in shipmentQuantityUnits
                select new SelectListItem
                {
                    Text = units.ToString(),
                    Value = ((int)units).ToString()
                };

            // Get the enum fields if present.
            var fields = typeof(ShipmentQuantityUnits).GetFields(BindingFlags.Public | BindingFlags.Static);
            var fieldNames = new Dictionary<string, int>();

            foreach (var field in fields)
            { 
                // Get the display attributes for the enum.
                var displayAttribute = (DisplayAttribute)field.GetCustomAttributes(typeof(DisplayAttribute)).SingleOrDefault();

                // Set field name to either the enum name or the display name.
                var name = (displayAttribute == null) ? field.Name : displayAttribute.Name;

                fieldNames.Add(name, (int)field.GetValue(ShipmentQuantityUnits.Tonnes));
            }

            var selectList = new SelectList(fieldNames, "Value", "Key");
            model.UnitsSelectList = selectList;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Info(ShipmentInfoViewModel model)
        {
            return View(model);
        }
    }
}