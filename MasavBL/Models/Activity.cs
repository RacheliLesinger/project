using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasavBL.Models
{
    public class Activity
    {
        public Activity()
        {
            this.Paying = new HashSet<Paying>();
            this.Customers = new HashSet<Customer>();
        }

        public override string ToString()
        {
            return Name;
        }

        [DisplayName("קוד פעילות")]
        [Key]
        public int Id { get; set; }
        [DisplayName("שם פעילות")]
        [Column("Name", TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }

        public virtual ICollection<Paying> Paying { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }

    }
}