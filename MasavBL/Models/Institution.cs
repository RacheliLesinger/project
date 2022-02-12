using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasavBL.Models
{
    public class Institution
    {
        public Institution()
        {
            this.Customers = new HashSet<Customer>();
        }

        [DisplayName("קוד המוסד השולח")]
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("שם המוסד השולח")]
        public string Name { get; set; }
        [Required]
        [DisplayName("מזהה המוסד השולח")]
        public string Code { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
    }
}
