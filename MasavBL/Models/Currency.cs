using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasavBL.Models
{
    public class Currency
    {
        public Currency()
        {
            this.Paying = new HashSet<Paying>();
            this.CurrencyRates = new HashSet<CurrencyRates>();
        }

        public override string ToString()
        {
            return Name;
        }

        [DisplayName("קוד מטבע")]
        [Key]
        public int Id { get; set; }
        [DisplayName("שם מטבע")]
        public string Name { get; set; }

        public virtual ICollection<Paying> Paying { get; set; }
        public virtual ICollection<CurrencyRates> CurrencyRates { get; set; }
    }
}