using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasavBL.Models
{
    public class Customer
    {
        public Customer()
        {
            this.BroadcastHistory = new HashSet<BroadcastHistory>();
            this.Paying = new HashSet<Paying>();
            this.PaymentHistory = new HashSet<PaymentHistory>();
        }
        public override string ToString()
        {
            return Name;
        }

        [DisplayName("קוד המוסד")]
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("שם המוסד")]
        public string Name { get; set; }
        [Required]
        [DisplayName("מזהה המוסד")]
        public string Code { get; set; }
        [DisplayName("תאריך הקמה")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> CreatedDate { get; set; }
        [Required]
        [DisplayName("קוד פעילות")]
        [EpplusIgnore]
        public Nullable<int> ActivityId { get; set; }
        [DisplayName("תאריך גביה 1")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        [Required]
        public int? PaymentDate1 { get; set; }
        [DisplayName("תאריך גביה 2")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        //[DataType(DataType.Date)]
        public int? PaymentDate2 { get; set; }
        [Required]
        [DisplayName("כתובת")]
        public string Address { get; set; }
        [DisplayName("איש קשר")]
        public string Contact { get; set; }
        [DisplayName("טלפון")]
        public string Phone { get; set; }
        [DisplayName("מייל")]
        public string Email { get; set; }

        //[Required]
        [DisplayName("קוד המוסד השולח")]
        [EpplusIgnore]
        public Nullable<int> InstitutionId { get; set; }



        [EpplusIgnore]
        public virtual ICollection<BroadcastHistory> BroadcastHistory { get; set; }
        [EpplusIgnore]
        public virtual ICollection<Paying> Paying { get; set; }
        [EpplusIgnore]
        public virtual ICollection<PaymentHistory> PaymentHistory { get; set; }
        [ForeignKey ("ActivityId")]
        [DisplayName("פעילות")]
        public virtual Activity Activity { get; set; }

        [ForeignKey("InstitutionId")]
        [DisplayName("המוסד השולח")]
        public virtual Institution Institution { get; set; }

    }
}