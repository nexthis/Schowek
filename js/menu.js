window.onload=function(){
    window.jQuery?$(document).ready(function(){

        $(".sidebarNavigation .navbar-collapse").hide().clone().appendTo("body").removeAttr("class").addClass("sideMenu").show(),

        $("#rightMenu a[data-href]").on('click', function() {

            var offset = 40;
            var duration = 1500;
            var link = this;
            $(".sideMenu ").removeClass("open");
            $(".sideMenu, .overlay").removeClass("open");
            $("#rightMenu a").removeClass("active");
            $('html, body').animate({
                scrollTop: $($(link).data('href')).offset().top - offset 
            }, duration);
           // $(link).addClass("active");
            return false;
        });

        $("body").append("<div class='overlay'></div>"),

        $(".sideMenu").addClass($(".sidebarNavigation").attr("data-sidebarClass")),

        $(".navbar-toggle, .navbar-toggler").on("click",function(){
            $(".sideMenu, .overlay").toggleClass("open"),$(".overlay").on("click",function(){
                $(this).removeClass("open"),$(".sideMenu").removeClass("open")
            })})

        ,$("body").on("click",".sideMenu.open .nav-item",function(){
            $(this).hasClass("dropdown")||$(".sideMenu, .overlay").toggleClass("open")
        })

        ,$(window).resize(function(){
            $("#rightMenuToggler").is(":hidden")?$(".sideMenu, .overlay").hide():$(".sideMenu, .overlay").show()
        })

    }):console.log("sidebarNavigation Requires jQuery")
};