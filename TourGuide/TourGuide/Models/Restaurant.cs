//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourGuide.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Restaurant
    {
        
        public int ID { get; set; }
        [Display(Name="City")]
        public int CityID { get; set; }
        [Display(Name="Restaurant")]
        public string Name { get; set; }
        public string Address { get; set; }
        [Display(Name="Zip Code")]
        public Nullable<int> ZipCode { get; set; }
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        public string URL { get; set; }
        public Nullable<int> Rating { get; set; }
        [Display(Name = "Latitude")]
        public Nullable<decimal> Lat { get; set; }
        [Display(Name = "Longitude")]
        public Nullable<decimal> Long { get; set; }
    
        public virtual Location Location { get; set; }
    }
}
