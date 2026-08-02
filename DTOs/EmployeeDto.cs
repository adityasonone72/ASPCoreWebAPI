namespace ASPCoreWebAPI.DTOs
{
    public class EmployeeDto //EmployeeDto will be useful to send data from server to client. Validation is important while validating input request.
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public double Salary { get; set; }
    }
}
