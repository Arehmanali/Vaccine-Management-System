using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace VaccineManagementSystem.Models
{
    [Table("Vaccine")]
    public class Vaccine
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string VaccineName { get; set; }
        
        [Required]
        [DisplayName("Doses Required")]
        public int DosesRequired { get; set; }

        [DisplayName("Days Between Dose")]
        public Nullable<int> DaysBetween { get; set; }

        [DisplayName("Total Dose Recieved")]
        public Nullable<int> TotalDoseRecieved { get; set; }

        [DisplayName("Total Dose Left")]
        public Nullable<int> TotalDoseLeft { get; set; }
    }
}
