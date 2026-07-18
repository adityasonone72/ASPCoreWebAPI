using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.ComponentModel.DataAnnotations;

namespace ASPCoreWebAPI.Models
{
    public class Employee
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(18, 55)]
        public int Age { get; set; }

        [Range(10000.0, double.MaxValue)]
        public decimal Salary { get; set; }
    }
}
