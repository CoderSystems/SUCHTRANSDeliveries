//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Inc2SuchTrans.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deliveryjob
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Deliveryjob()
        {
            this.JobQueue = new HashSet<JobQueue>();
            this.TrackDelivery = new HashSet<TrackDelivery>();
        }
    
        public int JobID { get; set; }
        public Nullable<int> DelID { get; set; }
        public Nullable<int> TruckID { get; set; }
        public Nullable<int> DriverID { get; set; }
        public string JobStatus { get; set; }
        public Nullable<bool> PortDelay { get; set; }
    
        public virtual Delivery Delivery { get; set; }
        public virtual TruckDriver TruckDriver { get; set; }
        public virtual Fleet Fleet { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JobQueue> JobQueue { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TrackDelivery> TrackDelivery { get; set; }
    }
}
