using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;

using Aries.Models;

public static class BreadcrumbHelper
{
    public static List<BreadcrumbItem> BuildBreadcrumb(this IUrlHelper urlHelper, ViewContext context)
    {
        var breadcrumbs = new List<BreadcrumbItem>();
        var controller = context.RouteData.Values["controller"].ToString();
        var action = context.RouteData.Values["action"].ToString();

        // Add Home
        breadcrumbs.Add(new BreadcrumbItem
        {
            Text = "Home",
            Url = urlHelper.Action("Index", "Home"),
            IsActive = false
        });

        // Add Controller
        if (controller != "Home")
        {
            breadcrumbs.Add(new BreadcrumbItem
            {
                Text = controller,
                Url = urlHelper.Action("Index", controller),
                IsActive = false
            });
        }

        // Add Action
        if (action != "Index")
        {
            breadcrumbs.Add(new BreadcrumbItem
            {
                Text = action,
                Url = "#",
                IsActive = true
            });
        }

        return breadcrumbs;
    }

    public static string GetCurrentPageTitle(this ViewContext context)
    {
        var controller = context.RouteData.Values["controller"].ToString();
        var action = context.RouteData.Values["action"].ToString();
        
        return action == "Index" ? controller : $"{controller} - {action}";
    }
}