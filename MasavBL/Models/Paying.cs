using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasavBL.Models
{
    [EpplusTable]
    public class Paying
    {
        public Paying()
        {
            this.PaymentHistory = new HashSet<PaymentHistory>();
        }
        [EpplusIgnore]
        [Required]
        [DisplayName("קוד משלם")]
        [Key]
        public int Id { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("קוד מוסד")]
        public int CustomerId { get; set; }

        [Required]
        [DisplayName("תעודת זהות")]
        public string IdentityNumber { get; set; }

        [Required]
        [DisplayName("שם משלם")]
        public string Name { get; set; }

        [EpplusIgnore]
        [Required]
        [DisplayName("קוד מטבע")]
        public Nullable<int> CurrencyId { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("קוד פעילות")]
        public Nullable<int> ActivityId { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("תאריך תשלום")] // יום בחודש
        [Range(1,31)]
        public int PaymentDate { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("מס' פעמים")]
        public Nullable<int> PaymentSum { get; set; }
        [Required]
        [DisplayName("סכום")]
        public Nullable<Double> Amount { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("קוד בנק")]
        public Nullable<int> CodeBankId { get; set; }
        [DisplayName("מס' סניף")]
        public string BankBranchNumber { get; set; }
        [Required]
        [DisplayName("מס' חשבון")]
        public string BankAccountNumber { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("תאריך התחלה")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> StartDate { get; set; }
        [EpplusIgnore]
        [Required]
        [DisplayName("תאריך סיום")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public Nullable<System.DateTime> EndDate { get; set; }

        [EpplusIgnore]
        [DefaultValue(true)]
        [Required]
        //מסמן האם הלקוח חדש - בפעם הראשונה שמפיקים לו דוח זה נכבה(False)
        public bool IsNew { get; set; }

        [DisplayName("בנק")]
        [ForeignKey("CodeBankId")]
        public virtual CodeBank CodeBank { get; set; }

        [Range(0, 99, ErrorMessage = "הכנס מספר בין 1-99")]
        [DisplayName("מחלקה")]
        public int Class { get; set; }


       

        [EpplusIgnore]
        [DisplayName("פעילות")]
        [ForeignKey("ActivityId")]
        public virtual Activity Activity { get; set; }
        [EpplusIgnore]
        [DisplayName("מטבע")]
        [ForeignKey("CurrencyId")]
        public virtual Currency Currency { get; set; }
        [EpplusIgnore]
        [DisplayName("מוסד")]
        [ForeignKey("CustomerId")]
         public virtual Customer Customers { get; set; }
        [EpplusIgnore]
        [DisplayName("היסטוריית תשלומים")]
        public virtual ICollection<PaymentHistory> PaymentHistory { get; set; }
        
    }
}