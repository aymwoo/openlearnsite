/* Adjust sidebar sticky top dynamically:
   if the lessonav is hidden (no cid/lid), drop to topbar-only offset */
(function () {
    var nav = document.getElementById("scm-lessonav");
    var sidebar = document.querySelector(".prog-sidebar");
    if (!sidebar) return;
    if (nav && nav.getAttribute("data-empty") === "1") {
        sidebar.style.top = "64px"; /* topbar 56px + 8px gap */
    }
})();
