﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class CreateMovementsViewModelResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CreateMovementsViewModelResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.NotificationMovements.ViewModels.Create.CreateMovementsViewModel" +
                            "Resources", typeof(CreateMovementsViewModelResources).Assembly);
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
        ///   Looks up a localized string similar to Actual quantity.
        /// </summary>
        public static string ActualQuantity {
            get {
                return ResourceManager.GetString("ActualQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You can&apos;t enter any symbols apart from a decimal full stop.
        /// </summary>
        public static string ActualQuantityIsValid {
            get {
                return ResourceManager.GetString("ActualQuantityIsValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The actual quantity must be a positive value.
        /// </summary>
        public static string ActualQuantityPositive {
            get {
                return ResourceManager.GetString("ActualQuantityPositive", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a value with less than {0} decimal places.
        /// </summary>
        public static string ActualQuantityPrecision {
            get {
                return ResourceManager.GetString("ActualQuantityPrecision", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a number.
        /// </summary>
        public static string ActualQuantityRequired {
            get {
                return ResourceManager.GetString("ActualQuantityRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to For example, .
        /// </summary>
        public static string DateHint {
            get {
                return ResourceManager.GetString("DateHint", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The actual date of shipment cannot be in the past. Please enter a different date..
        /// </summary>
        public static string DateNotInPast {
            get {
                return ResourceManager.GetString("DateNotInPast", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid date.
        /// </summary>
        public static string DateValid {
            get {
                return ResourceManager.GetString("DateValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number in the &apos;Day&apos; field.
        /// </summary>
        public static string DayRequired {
            get {
                return ResourceManager.GetString("DayRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a value that doesn&apos;t exceed the total consented quantity.
        /// </summary>
        public static string HasExceededTotalQuantity {
            get {
                return ResourceManager.GetString("HasExceededTotalQuantity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number in the &apos;Month&apos; field.
        /// </summary>
        public static string MonthRequired {
            get {
                return ResourceManager.GetString("MonthRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Number of shipments.
        /// </summary>
        public static string NumberToCreate {
            get {
                return ResourceManager.GetString("NumberToCreate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter the number of shipments.
        /// </summary>
        public static string NumberToCreateRequired {
            get {
                return ResourceManager.GetString("NumberToCreateRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number of shipments.
        /// </summary>
        public static string NumberToCreateValid {
            get {
                return ResourceManager.GetString("NumberToCreateValid", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please select at least one packaging type.
        /// </summary>
        public static string PackagingTypeRequired {
            get {
                return ResourceManager.GetString("PackagingTypeRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a valid number in the &apos;Year&apos; field.
        /// </summary>
        public static string YearRequired {
            get {
                return ResourceManager.GetString("YearRequired", resourceCulture);
            }
        }
    }
}