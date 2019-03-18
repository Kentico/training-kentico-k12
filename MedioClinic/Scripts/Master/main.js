document.addEventListener("DOMContentLoaded", function () {
    var sidenavElement = document.querySelector(".sidenav");
    M.Sidenav.init(sidenavElement);
    var dropdownElement = document.querySelector(".dropdown-trigger");

    M.Dropdown.init(dropdownElement, {
        hover: false
    });
});