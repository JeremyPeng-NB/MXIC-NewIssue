namespace MXIC_PCCS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("View_Swipe")]
    public partial class View_Swipe
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string PoNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string VendorName { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string EmpID { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string EmpName { get; set; }

        [Key]
        [Column(Order = 4)]
        public DateTime? SwipeTime { get; set; }
        [Key]
        [Column(Order = 5)]
        
        public Guid EditID { get; set; }

        [Key]
        [Column(Order = 6)]

        public string AttendType { get; set; }

        [Key]
        [Column(Order = 7)]

        public float Hour { get; set; }


            [Key]
            [Column(Order = 8)]

            public string CheckType { get; set; }

        [Column(Order = 9)]
        public string WorkShift { get; set; }
    }
}
