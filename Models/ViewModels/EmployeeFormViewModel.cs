using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

public class EmployeeFormViewModel
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [Display(Name = "Department")]
    public int DepartmentId { get; set; }
    
    public IEnumerable<SelectListItem> Departments { get; set; } = new List<SelectListItem>();
}