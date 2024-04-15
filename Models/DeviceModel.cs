namespace ITInventoryManagementAPI.Models
{
public class Device
{
    
    public int Id { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
}
