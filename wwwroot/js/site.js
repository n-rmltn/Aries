// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
  const body = document.querySelector("body");
  const sidebarToggle = document.getElementById("sidebar-toggle");
  const sidebar = document.querySelector(".sidebar");

  // Apply sidebar state based on local storage
  function applySidebarState() {
    const sidebarCollapsed =
      localStorage.getItem("sidebar-collapsed") === "true";
    if (window.innerWidth <= 768) {
      body.classList.toggle("sidebar-collapsed", !sidebarCollapsed);
    } else {
      body.classList.toggle("sidebar-collapsed", sidebarCollapsed);
    }
  }

  // Init
  applySidebarState();

  // Resize apply
  window.addEventListener("resize", applySidebarState);

  // Toggle handler
  sidebarToggle?.addEventListener("click", () => {
    body.classList.toggle("sidebar-collapsed");
    localStorage.setItem(
      "sidebar-collapsed",
      body.classList.contains("sidebar-collapsed")
    );
  });

  // Click outside
  document.addEventListener("click", (e) => {
    if (window.innerWidth <= 768) {
      if (
        !sidebar.contains(e.target) &&
        !sidebarToggle.contains(e.target) &&
        body.classList.contains("sidebar-collapsed")
      ) {
        body.classList.remove("sidebar-collapsed");
        localStorage.setItem("sidebar-collapsed", "true");
      }
    }
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
