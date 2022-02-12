using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasavBL.Models
{
    public class CurrencyRates
    {
        [DisplayName("קוד שערי מטבע")]
        [Key]
        public int Id { get; set; }
        [DisplayName("קוד מטבע")]
        public int CurrencyId { get; set; }
        [DisplayName("תאריך")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> Date { get; set; }
        [DisplayName("שער מטבע בשקלים")]
        public Nullable<double> CurrencyRateInShekels { get; set; }
        [DisplayName("מטבע")]
        public virtual Currency Currency { get; set; }
    }
}