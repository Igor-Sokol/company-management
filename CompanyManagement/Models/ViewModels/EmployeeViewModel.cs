using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyManagement.Models.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Patronymic { get; set; }

        [Required]
        [Display(Name = "Hire Date")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        [Required]
        public Post Post { get; set; }

        [Required]
        public CompanyViewModel Company { get; set; }
    }
}
