using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasavBL.Models
{
    public class PaymentHistory
    {
        [DisplayName("קוד היסטוריית תשלומים")]
        [Key]
        public int Id { get; set; }
        [DisplayName("קוד משלם")]
        public int PaidId { get; set; }
        [DisplayName("קוד מוסד")]
        public int CustomerId { get; set; }
        [DisplayName("תאריך תשלום")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> PaymentDate { get; set; }
        [DisplayName("סכום תשלום")]
        public Nullable<int> PaymentAmount { get; set; }
        [DisplayName("מוסד")]
        public virtual Customer Customers { get; set; }
        [DisplayName("משלם")]
        public virtual Paying Paying { get; set; }
    }
}