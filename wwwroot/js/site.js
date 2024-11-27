// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
  const body = document.querySelector("body");
  const sidebarToggle = document.getElementById("sidebar-toggle");
  const sidebar = document.querySelector(".sidebar");

  // Load saved state based on viewport
  function applySidebarState() {
    const sidebarCollapsed =
      localStorage.getItem("sidebar-collapsed") === "true";
    if (window.innerWidth <= 768) {
      // Mobile: sidebar collapsed by default unless explicitly opened
      body.classList.toggle("sidebar-collapsed", !sidebarCollapsed);
    } else {
      // Desktop: sidebar open by default unless explicitly collapsed
      body.classList.toggle("sidebar-collapsed", sidebarCollapsed);
    }
  }

  // Apply initial state
  applySidebarState();

  // Reapply on resize
  window.addEventListener("resize", applySidebarState);

  // Toggle handler
  sidebarToggle?.addEventListener("click", () => {
    body.classList.toggle("sidebar-collapsed");
    localStorage.setItem(
      "sidebar-collapsed",
      body.classList.contains("sidebar-collapsed")
    );
  });

  // Click outside handler for mobile
  document.addEventListener("click", (e) => {
    if (window.innerWidth <= 768) {
      // If sidebar is visible (shown) and click is outside
      if (
        !sidebar.contains(e.target) &&
        !sidebarToggle.contains(e.target) &&
        body.classList.contains("sidebar-collapsed") // In mobile, collapsed means shown
      ) {
        body.classList.remove("sidebar-collapsed"); // Hide sidebar
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
