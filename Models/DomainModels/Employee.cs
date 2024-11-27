namespace Aries.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Foreign key
    public int DepartmentId { get; set; }
    
    // Navigation property
    public virtual Department Department { get; set; } = null!;
}