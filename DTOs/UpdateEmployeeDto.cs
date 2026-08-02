using System.ComponentModel.DataAnnotations;

namespace ASPCoreWebAPI.DTOs
{
    public class UpdateEmployeeDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [RegularExpression(@"^[a-zA-Z\s]+$")]
        public string Name { get; set; }

        //[Required] if we are using range then why required
        [Range(18, 60)]
        public int Age { get; set; }

        //[Required]
        [Range(10000, double.MaxValue)]
        public double Salary { get; set; }
    }
}
