﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.ImportNotification.Views.NumberOfShipments {
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
    public class ConfirmResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ConfirmResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.ImportNotification.Views.NumberOfShipments.ConfirmResources", typeof(ConfirmResources).Assembly);
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
        ///   Looks up a localized string similar to This increase will require the Notifier to pay an additional charge of {0}.
        /// </summary>
        public static string AdditionalCharge {
            get {
                return ResourceManager.GetString("AdditionalCharge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Do not make the change and return to notification overview.
        /// </summary>
        public static string CancelButtonText {
            get {
                return ResourceManager.GetString("CancelButtonText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Decrease.
        /// </summary>
        public static string Decrease {
            get {
                return ResourceManager.GetString("Decrease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are about to decrease the number of shipments for this notification from.
        /// </summary>
        public static string DecreaseTo {
            get {
                return ResourceManager.GetString("DecreaseTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Once this change has been committed shipments will decrease to {0} and the Notifier will only be able to make up to {0} shipments on this notification.
        /// </summary>
        public static string GuidanceDecrease {
            get {
                return ResourceManager.GetString("GuidanceDecrease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Once this change has been committed shipments for this notification will increase to {0} and allow the Notifier to make up to {0} shipments.
        /// </summary>
        public static string GuidanceNoIncrease {
            get {
                return ResourceManager.GetString("GuidanceNoIncrease", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to the total number of intended shipments.
        /// </summary>
        public static string Heading {
            get {
                return ResourceManager.GetString("Heading", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Increase.
        /// </summary>
        public static string Increase {
            get {
                return ResourceManager.GetString("Increase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are about to increase the number of shipments for this notification from.
        /// </summary>
        public static string IncreaseTo {
            get {
                return ResourceManager.GetString("IncreaseTo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This charge will need to be paid prior to the Notifier using these additional shipments.
        /// </summary>
        public static string NeedToPay {
            get {
                return ResourceManager.GetString("NeedToPay", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This does not change the charge for this notification.
        /// </summary>
        public static string NoChargeChange {
            get {
                return ResourceManager.GetString("NoChargeChange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Confirm number of shipments change.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
