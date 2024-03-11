document.addEventListener("DOMContentLoaded", function (event) {
    const showNavbar = (toggleId, navId, bodyId, headerId) => {
        const toggle = document.getElementById(toggleId),
            nav = document.getElementById(navId),
            bodypd = document.getElementById(bodyId),
            headerpd = document.getElementById(headerId);

        // Function to toggle sidebar and padding
        const toggleSidebar = () => {
            nav.classList.toggle('resize');
            toggle.classList.toggle('bx-x');
            bodypd.classList.toggle('body-pd');
            headerpd.classList.toggle('header-pd');
        };

        // Validate that all variables exist
        if (toggle && nav && bodypd && headerpd) {
            toggle.addEventListener('click', toggleSidebar);
        }

        // Function to check window width and auto-hide navbar
        //const autoHideNavbar = () => {
        //    if (window.innerWidth < 768) {
        //        toggleSidebar(); // Hide sidebar if the initial width is less than 768 pixels
        //    }
        //};

        // Call autoHideNavbar on page load
       /* autoHideNavbar();*/

        // Add a resize event listener to handle changes in window width
        /*window.addEventListener('resize', autoHideNavbar);*/
    };

    showNavbar('header-toggle', 'nav-bar', 'body-pd','header');

    /*===== LINK ACTIVE =====*/
    const linkColor = document.querySelectorAll('.nav_link');

    function colorLink() {
        if (linkColor) {
            linkColor.forEach(l => l.classList.remove('active'));
            this.classList.add('active');
        }
    }

    linkColor.forEach(l => l.addEventListener('click', colorLink));
});