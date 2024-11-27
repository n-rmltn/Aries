using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

using Aries.Models;
using Aries.Repositories;
using Aries.Services.Interfaces;

namespace Aries.Controllers;


[Authorize]
public class EmployeeController : Controller
{
    private readonly IEmployeeService _employeeService;
    private readonly IDepartmentService _departmentService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(
        IEmployeeService employeeService,
        IDepartmentService departmentService,
        ILogger<EmployeeController> logger)
    {
        _employeeService = employeeService;
        _departmentService = departmentService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllAsync();
        return View(employees);
    }

    public async Task<IActionResult> Create()
    {
        var departments = await _departmentService.GetAllAsync();
        var model = new EmployeeFormViewModel
        {
            Departments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            })
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var departments = await _departmentService.GetAllAsync();
            model.Departments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
            return View(model);
        }

        if (await _employeeService.CreateAsync(model))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = "Employee created successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Failed to create employee");
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var model = await _employeeService.GetForEditAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var departments = await _departmentService.GetAllAsync();
            model.Departments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            });
            return View(model);
        }

        if (await _employeeService.UpdateAsync(model))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = "Employee updated successfully";
            return RedirectToAction(nameof(Index));
        }

        ModelState.AddModelError("", "Failed to update employee");
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        if (await _employeeService.DeleteAsync(id))
        {
            TempData["ToastTitle"] = "Success";
            TempData["ToastMessage"] = "Employee deleted successfully";
        }
        else
        {
            TempData["ToastTitle"] = "Error";
            TempData["ToastMessage"] = "Failed to delete employee";
        }
        return RedirectToAction(nameof(Index));
    }
}