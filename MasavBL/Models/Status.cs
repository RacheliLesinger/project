using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasavBL.Models
{
    public class Status
    { 
         public Status()
        {
            this.PaymentHistories = new HashSet<PaymentHistory>();
            this.BroadcastHistories = new HashSet<BroadcastHistory>();
        }

        public override string ToString()
        {
            return Name;
        }

        [DisplayName("קוד סטטוס")]
        [Key]
        public int Id { get; set; }
        [DisplayName("שם סטטוס")]
        [Column("Name", TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }


        public virtual ICollection<BroadcastHistory> BroadcastHistories { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }

    }
}