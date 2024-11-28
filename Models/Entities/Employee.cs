namespace Aries.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // FK
    public int DepartmentId { get; set; }
    
    // Nav
    public virtual Department Department { get; set; } = null!;
}