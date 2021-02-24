namespace MXIC_PCCS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SwipeDouble")]
    public partial class SwipeDouble
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string PoNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string VendorName { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string EmpID { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(50)]
        public string CheckType { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 5)]
        public DateTime SwipeTime { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string WorkShift { get; set; }
    }
}
