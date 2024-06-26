﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CaptureViewModelResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CaptureViewModelResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.AdminExportNotificationMovements.ViewModels.CaptureMovement.Capt" +
                            "ureViewModelResources", typeof(CaptureViewModelResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;actual date of shipment&quot; cannot be before the prenotification date. Please enter a different date..
        /// </summary>
        public static string ActualDateBeforePrenotification {
            get {
                return ResourceManager.GetString("ActualDateBeforePrenotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;actual date of shipment&quot; should not be more than 30 calendar days after the &quot;prenotification date&quot;. Please advise the exporter or enter a different date..
        /// </summary>
        public static string ActualDateGreaterthanSixtyDays {
            get {
                return ResourceManager.GetString("ActualDateGreaterthanSixtyDays", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Actual date of shipment.
        /// </summary>
        public static string ActualDateLabel {
            get {
                return ResourceManager.GetString("ActualDateLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter the actual date of shipment.
        /// </summary>
        public static string ActualDateRequired {
            get {
                return ResourceManager.GetString("ActualDateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Comments.
        /// </summary>
        public static string Comments {
            get {
                return ResourceManager.GetString("Comments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do you want to enter comments against this shipment?.
        /// </summary>
        public static string HasComments {
            get {
                return ResourceManager.GetString("HasComments", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No prenotification received.
        /// </summary>
        public static string HasNoPrenotification {
            get {
                return ResourceManager.GetString("HasNoPrenotification", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shipment {0}.
        /// </summary>
        public static string NewShipmentNumber {
            get {
                return ResourceManager.GetString("NewShipmentNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shipment number.
        /// </summary>
        public static string Number {
            get {
                return ResourceManager.GetString("Number", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number.
        /// </summary>
        public static string NumberIsInt {
            get {
                return ResourceManager.GetString("NumberIsInt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a shipment number.
        /// </summary>
        public static string NumberRequired {
            get {
                return ResourceManager.GetString("NumberRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Prenotification date.
        /// </summary>
        public static string PrenotificationDateLabel {
            get {
                return ResourceManager.GetString("PrenotificationDateLabel", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please tick the box or enter prenotification received date.
        /// </summary>
        public static string PrenotificationDateRequired {
            get {
                return ResourceManager.GetString("PrenotificationDateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;prenotification date&quot; cannot be in the future. Please enter a different date..
        /// </summary>
        public static string PrenotifictaionDateInfuture {
            get {
                return ResourceManager.GetString("PrenotifictaionDateInfuture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the actual quantity.
        /// </summary>
        public static string QuantityRequired {
            get {
                return ResourceManager.GetString("QuantityRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide receipt information before providing date of {0}.
        /// </summary>
        public static string ReceiptMustBeCompletedFirst {
            get {
                return ResourceManager.GetString("ReceiptMustBeCompletedFirst", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;when the waste was received&quot; date cannot be before the &quot;actual date of shipment&quot;. Please enter a different date..
        /// </summary>
        public static string ReceivedDateBeforeActualDate {
            get {
                return ResourceManager.GetString("ReceivedDateBeforeActualDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;when the waste was received&quot; date cannot be in the future. Please enter a different date..
        /// </summary>
        public static string ReceivedDateInfuture {
            get {
                return ResourceManager.GetString("ReceivedDateInfuture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the received date.
        /// </summary>
        public static string ReceivedDateRequired {
            get {
                return ResourceManager.GetString("ReceivedDateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;when was the waste {0}&quot; date cannot be before the &quot;when was the waste received &quot; date. Please enter a different date..
        /// </summary>
        public static string RecoveredDateBeforeReceivedDate {
            get {
                return ResourceManager.GetString("RecoveredDateBeforeReceivedDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The &quot;when the waste was {0}&quot; date cannot be in the future. Please enter a different date..
        /// </summary>
        public static string RecoveredDateInfuture {
            get {
                return ResourceManager.GetString("RecoveredDateInfuture", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do not provide {0} date when the prenotification has been rejected.
        /// </summary>
        public static string RecoveryDateCannotBeEnteredForRejected {
            get {
                return ResourceManager.GetString("RecoveryDateCannotBeEnteredForRejected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide the rejected quantity.
        /// </summary>
        public static string RejectQuantityRequired {
            get {
                return ResourceManager.GetString("RejectQuantityRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please provide a reason for rejection.
        /// </summary>
        public static string RejectReasonRequired {
            get {
                return ResourceManager.GetString("RejectReasonRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to WSR/Basel annual stats marking (when required).
        /// </summary>
        public static string StatsMarking {
            get {
                return ResourceManager.GetString("StatsMarking", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select annual stats marking for rejection.
        /// </summary>
        public static string StatsMarkingRequired {
            get {
                return ResourceManager.GetString("StatsMarkingRequired", resourceCulture);
            }
        }
    }
}
