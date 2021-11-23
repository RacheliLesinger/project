using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MasavBL.Models
{
    public class CodeBank
    {
        public CodeBank()
        {
            this.Paying = new HashSet<Paying>();
        }
        public override string ToString()
        {
            return Name;
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual ICollection<Paying> Paying { get; set; }
    }
}