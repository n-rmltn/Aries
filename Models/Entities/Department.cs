namespace Aries.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    // Nav
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}