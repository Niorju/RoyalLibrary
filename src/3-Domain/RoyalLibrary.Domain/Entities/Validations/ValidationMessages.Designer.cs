﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RoyalLibrary.Domain.Entities.Validations {
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
    internal class ValidationMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ValidationMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("RoyalLibrary.Domain.Entities.Validations.ValidationMessages", typeof(ValidationMessages).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Book Updated.
        /// </summary>
        internal static string BookUpdated {
            get {
                return ResourceManager.GetString("BookUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Category is Required.
        /// </summary>
        internal static string CategoryRequired {
            get {
                return ResourceManager.GetString("CategoryRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to FirstName is Required.
        /// </summary>
        internal static string FirstNameRequired {
            get {
                return ResourceManager.GetString("FirstNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Book inserted.
        /// </summary>
        internal static string InsertBook {
            get {
                return ResourceManager.GetString("InsertBook", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Isbn is Required.
        /// </summary>
        internal static string IsbnRequired {
            get {
                return ResourceManager.GetString("IsbnRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last name is Required.
        /// </summary>
        internal static string LastNameRequired {
            get {
                return ResourceManager.GetString("LastNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Title is Required.
        /// </summary>
        internal static string TitleRequired {
            get {
                return ResourceManager.GetString("TitleRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TotalCopies must ben grander than 0.
        /// </summary>
        internal static string TotalCopiesSizeRequired {
            get {
                return ResourceManager.GetString("TotalCopiesSizeRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TypeIs is Required.
        /// </summary>
        internal static string TypeIsRequired {
            get {
                return ResourceManager.GetString("TypeIsRequired", resourceCulture);
            }
        }
    }
}