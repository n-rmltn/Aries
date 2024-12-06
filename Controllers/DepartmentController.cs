using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Aries.Models;
using Aries.Services.Interfaces;

namespace Aries.Controllers;

[Authorize]
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(
        IDepartmentService departmentService,
        ILogger<DepartmentController> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetDepartments()
    {
        var departments = await _departmentService.GetAllAsync();
        return Json(new { data = departments });
    }

    public IActionResult Create()
    {
        return View(new Department());
    }

    [HttpPost]
    public async Task<IActionResult> Create(Department department)
    {
        if (!ModelState.IsValid) return View(department);

        if (await _departmentService.CreateAsync(department))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = "Department created successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Failed to create department");
        return View(department);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);
        if (department == null) return NotFound();
        return View(department);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Department department)
    {
        if (!ModelState.IsValid) return View(department);

        if (await _departmentService.UpdateAsync(department))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = "Department updated successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Failed to update department");
        return View(department);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _departmentService.DeleteAsync(id))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = "Department deleted successfully";
        }
        else
        {
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Cannot delete department with employees";
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> BulkDelete(string ids)
    {
        if (string.IsNullOrEmpty(ids))
        {
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "No departments selected";
            return RedirectToAction(nameof(Index));
        }

        var idList = ids.Split(',').Select(int.Parse).ToList();
        
        if (await _departmentService.BulkDeleteAsync(idList))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = $"Successfully deleted {idList.Count} departments";
        }
        else
        {
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Cannot delete departments with employees";
        }

        return RedirectToAction(nameof(Index));
    }
}