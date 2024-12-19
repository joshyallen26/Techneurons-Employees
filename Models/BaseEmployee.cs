using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employee.Models
{
    public class BaseEmployee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Basic Pay is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Basic Pay must be a positive value.")]
        public decimal BasicPay { get; set; }

        [Required(ErrorMessage = "Allowances are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Allowances must be a positive value.")]
        public decimal Allowances { get; set; }

        [Required(ErrorMessage = "Deductions are required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Deductions must be a positive value.")]
        public decimal Deductions { get; set; }

     
        public decimal CalculateSalary()
        {
            return BasicPay + Allowances - Deductions;
        }
    }

    public class Manager : BaseEmployee
    {
        public string Department { get; set; }
    }

   
    public class Developer : BaseEmployee
    {
        public string ProgrammingLanguage { get; set; }
    }

    
    public class Intern : BaseEmployee
    {
        public string SchoolName { get; set; }
    }
}