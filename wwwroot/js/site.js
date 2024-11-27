// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
  const body = document.querySelector("body");
  const sidebarToggle = document.getElementById("sidebar-toggle");
  const sidebar = document.querySelector(".sidebar");

  // Load saved state
  const sidebarCollapsed = localStorage.getItem("sidebar-collapsed") === "true";
  if (sidebarCollapsed) {
    body.classList.add("sidebar-collapsed");
  }

  // Toggle handler
  sidebarToggle?.addEventListener("click", () => {
    body.classList.toggle("sidebar-collapsed");
    localStorage.setItem(
      "sidebar-collapsed",
      body.classList.contains("sidebar-collapsed")
    );
  });
});

function showToast(title, message) {
  const toastTitle = document.getElementById("toastTitle");
  const toastMessage = document.getElementById("toastMessage");
  const toast = document.getElementById("liveToast");

  toastTitle.textContent = title;
  toastMessage.textContent = message;

  const bsToast = new bootstrap.Toast(toast);
  bsToast.show();
}
