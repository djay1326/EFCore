function openNav() {
    document.getElementById("mySidenav").style.width = "250px";
}

function closeNav() {
    document.getElementById("mySidenav").style.width = "0";
}


window.onscroll = function () {
    if (window.pageYOffset > 100) {
        document.getElementById("navbar11").classList.add('scrolled');
        document.getElementById("navbar11").classList.add('fixed-top');
        document.getElementById("imgg2").setAttribute("src", "/images/small.png");
        
    }
    else {
        document.getElementById("navbar11").classList.remove('scrolled');
        document.getElementById("navbar11").classList.remove('fixed-top');
        document.getElementById("imgg2").setAttribute("src", "/images/large.png");
    }
}