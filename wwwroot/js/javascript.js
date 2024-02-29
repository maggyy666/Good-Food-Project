function toggleMenu() {
    var navbarMenu = document.querySelector('.navbar-menu');
    navbarMenu.classList.toggle('active');
}


    document.getElementById("joinButton").addEventListener("click", function () {
        var loginUrl = '@Url.Action("Login", "ControllerName")';
    window.location.href = loginUrl;
    });

