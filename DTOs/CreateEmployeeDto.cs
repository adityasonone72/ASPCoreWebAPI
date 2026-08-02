using System.ComponentModel.DataAnnotations;

namespace ASPCoreWebAPI.DTOs
{
    public class CreateEmployeeDto //these Dtos are used when client sends request body (e.g. in json), so id will be generated not sent by client
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
