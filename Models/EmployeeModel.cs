namespace ITInventoryManagementAPI.Models
{
public class Employee
{
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }

    public ICollection<Device> Devices { get; set; } = new List<Device>();
}

}
