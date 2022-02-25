using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasavBL.Models
{
    public class BroadcastHistory
    {
        [DisplayName("קוד היסטורית שידורים")]
        [Key]
        public int Id { get; set; }
        [DisplayName("קוד מוסד")]
        public int CustomerId { get; set; }
        [DisplayName("תאריך שידור")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> BroadcastDate { get; set; }
        [DisplayName("תאריך ערך")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> ValueDate { get; set; }
        [DisplayName("סכום שידור")]
        public Nullable<double> BroadcastAmount { get; set; }
        [DisplayName(" (משלמים)כמות רשומות")]
        public Nullable<int> SumRecords { get; set; }
        [DisplayName("כמות חדשים")]
        public Nullable<int> SumNewRecords { get; set; }
        [DisplayName("אסמכתת שידור")]
        public string BroadcastReference { get; set; }
        [DisplayName("הערות")]
        [DataType(DataType.Text)]
        public string Notes { get; set; }

        [DisplayName("מחלקה")]
        public int Class { get; set; }

        [DisplayName("קוד סטטוס")]
        public int StatusId { get; set; }


        [DisplayName("מוסד")]
        [ForeignKey("CustomerId")]
        public virtual Customer Customers { get; set; }

        [ForeignKey("StatusId")]
        [DisplayName("סטטוס")]
        public virtual Status Status { get; set; }
    }
}