﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EA.Iws.Web.Areas.NotificationApplication.Views.Importer {
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
    public class ImporterResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ImporterResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("EA.Iws.Web.Areas.NotificationApplication.Views.Importer.ImporterResources", typeof(ImporterResources).Assembly);
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
        ///   Looks up a localized string similar to Address.
        /// </summary>
        public static string Address {
            get {
                return ResourceManager.GetString("Address", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The correspondence address for the importer - consignee. Note that this may be different to the registered company address so ensure you use the appropriate address..
        /// </summary>
        public static string AddressDescription {
            get {
                return ResourceManager.GetString("AddressDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Contact person.
        /// </summary>
        public static string Contact {
            get {
                return ResourceManager.GetString("Contact", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This should be the contact person for the importer - consignee.
        /// </summary>
        public static string ContactDescription {
            get {
                return ResourceManager.GetString("ContactDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Who is the importer - consignee?.
        /// </summary>
        public static string Header {
            get {
                return ResourceManager.GetString("Header", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This information may be obtained from your importer – consignee. Enter &quot;Not applicable&quot; when no relevant registration number applies..
        /// </summary>
        public static string Message {
            get {
                return ResourceManager.GetString("Message", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Who is the importer - consignee?.
        /// </summary>
        public static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
