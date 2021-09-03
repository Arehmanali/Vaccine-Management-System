using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace VaccineManagementSystem.Models
{
    [Table("Patient")]
    public class Patient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Name")]
        public string PatientName { get; set; }

        [Required]
        [DisplayName("Vaccine Name")]
        public String VaccineName { get; set; }

        [Required]
        [DisplayName("1st Dose")]
        public String FirstDose { get; set; }

        [DisplayName("2nd Dose")]
        public String SecondDose { get; set; }
    }
}
